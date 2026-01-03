using UnityEngine;

namespace MapBuilder
{
    // This class shows a preview of the changes that the creative player is about to make.
    // It also applies the changes if the creative player confirms.
    public class EditorPiecePreview : MonoBehaviour
    {
        MapPiece piece;
        GameObject piecePrefab;
        GameObject pieceObject;
        bool pieceInstantiated = false;

        // floatingReachDistance applies to floating mode and force floating mode
        [SerializeField] private float floatingReachDistance = 10f;
        // nonFloatingReachDistance applies to place mode, delete mode, and stack mode.
        [SerializeField] private float nonFloatingReachDistance = 100f;
        private float gridUnitSize;

        private CreativePlayerCamera cameraScript;
        
        private bool alreadyRotated = false;
        private bool alreadyPlaced = false;

        private GameObject targetedPieceObject;

        private string _prefabName;
        private bool _prefabNameSet;

        public string prefabName { get => _prefabName; set { _prefabName = value; _prefabNameSet = true; pieceInstantiated = false; } }
        
        void Awake()
        {
            cameraScript = GetComponentInChildren<CreativePlayerCamera>();
        }

        void Start()
        {
            // The MapEditor singleton is assumed to be instantiated
            gridUnitSize = MapEditor.Instance.map.gridUnitSize;
        }

        void Update()
        {
            // Since the assets are loaded asynchronously, they might not be completely loaded at first, so we have to wait
            if (!MapEditor.Instance.assetsLoaded)
                return;

            // The creative player has to make a selection in the menu first.
            if (!_prefabNameSet)
                return;

            if (!pieceInstantiated)
            {
                // Maybe the pieceObject has already been previously instantiated, so we have to destroy it and make a new one.
                // This can happen when the player selects a new piece from the menu.
                if (pieceObject is not null)
                    Destroy(pieceObject);

                // Make a new piece GameObject by cloning a prefab.
                piecePrefab = MapEditor.Instance.mapPiecePrefabs[_prefabName];
                piece = new MapPiece(new Vector3Int(0, 0, 0), new Piece(_prefabName), 0);
                pieceObject = Instantiate(piecePrefab);
                pieceInstantiated = true;
            }

            // We don't want any pieces to get placed or removed while the player is clicking around the menu.
            if (MapEditorInputManager.Instance.menuOpen)
                return;

            // Make the piece GameObject active so the raycast will work properly. It might get deactivated immediately after.
            if (targetedPieceObject is not null)
                targetedPieceObject.SetActive(true);


            // These local variables get set depending on the edit mode.
            // currentReachDistance is the maximum distance from the player that a piece can be affected.
            float currentReachDistance = 0f;
            // Whether or not a piece can be placed in the exact same spot as another piece.
            bool stack = false;
            // Whether or not the preview shows up.
            bool hidePreview = false;
            // Whether or not a piece can be placed
            bool canPlacePiece = false;
            // Whenther or not a piece can be removed
            bool canRemovePiece = false;
            // Whether or not to hide the preview if the raycast misses
            bool hidePreviewOnMiss = false;
            // Whether or not to place a piece in currentReachDistance no matter what
            bool force = false;
            switch (MapEditorInputManager.Instance.editMode)
            {
                case (EditMode.place):
                    currentReachDistance = nonFloatingReachDistance;
                    stack = false;
                    hidePreview = false;
                    canPlacePiece = true;
                    hidePreviewOnMiss = true;
                    break;

                case (EditMode.floatingPlace):
                    currentReachDistance = floatingReachDistance;
                    stack = false;
                    hidePreview = false;
                    canPlacePiece = true;
                    break;

                case (EditMode.forceFloatingPlace):
                    currentReachDistance = floatingReachDistance;
                    force = true;
                    canPlacePiece = true;
                    break;

                case (EditMode.stack):
                    currentReachDistance = nonFloatingReachDistance;
                    stack = true;
                    hidePreview = false;
                    canPlacePiece = true;
                    hidePreviewOnMiss = true;
                    break;

                case (EditMode.remove):
                    currentReachDistance = nonFloatingReachDistance;
                    stack = true;
                    hidePreview = true;
                    canRemovePiece = true;
                    break;
            }

            bool colliderHit;
            Vector3Int gridTargetPosition;
            // Perform the raycast and get the target position on the 3D grid, whether or not the raycast hit something, and the object that was hit.
            gridTargetPosition = GetTargetGridPosition(
                    maxDistance:currentReachDistance,
                    forward:cameraScript.playerCamera.transform.forward,
                    origin:cameraScript.playerCamera.transform.position,
                    stack:stack,
                    out colliderHit,
                    out targetedPieceObject);

            // Force the target to be the reach distance of the player in the direction that the player is looking.
            if (force)
            {
                gridTargetPosition = GetGridPositionFromWorldPosition(
                        cameraScript.playerCamera.transform.position +
                        (currentReachDistance * cameraScript.playerCamera.transform.forward));
            }

            // Hide the preview if hidePreviewOnMiss and the raycast missed.
            if (!colliderHit && hidePreviewOnMiss)
            {
                hidePreview = true;
            }

            // Temporarily disable the GameObject hit by the raycast if we are in remove mode
            if (canRemovePiece && colliderHit)
            {
                targetedPieceObject.SetActive(false);
            }

            // Enable or disable the preview, then set the location of the preview.
            pieceObject.SetActive(!hidePreview);
            piece.location = gridTargetPosition;
            pieceObject.transform.position = WorldPositionFromGridPosition(gridTargetPosition);
        

            // Rotate the piece only once if the rotate key is pressed
            if (MapEditorInputManager.Instance.rotateAction.inProgress && alreadyRotated == false)
            {
                alreadyRotated = true;
                piece.orientation++;
                Quaternion newRotation = new Quaternion();
                newRotation.eulerAngles = new Vector3(0, piece.orientation * 90, 0);
                pieceObject.transform.rotation = newRotation;

            }
            else if (!MapEditorInputManager.Instance.rotateAction.inProgress && alreadyRotated == true)
            {
                alreadyRotated = false;
            }

            // Add or remove a piece from the map if the mouse button is clicked
            if (MapEditorInputManager.Instance.placeAction.inProgress && !alreadyPlaced)
            {
                alreadyPlaced = true;
                if (canPlacePiece && !hidePreview)
                {
                    AddPiece(piece, pieceObject);
                }
                else if (canRemovePiece)
                {
                    MapEditor.Instance.map.DeleteMapPieceByLocation(gridTargetPosition);
                    Destroy(targetedPieceObject);
                    targetedPieceObject = null;
                }
            }
            else if (!MapEditorInputManager.Instance.placeAction.inProgress && alreadyPlaced)
            {
                alreadyPlaced = false;
            }
        }

        // Add a new piece both to the Map object and to the world.
        private void AddPiece(MapPiece mapPiece, GameObject pieceObject)
        {
            // Store the piece to the Map object
            MapEditor.Instance.map.AddMapPiece(piece);
            // Create a clone of the preview.
            GameObject newPieceObject = Instantiate(pieceObject);

            // Create a new GameObject and attach a box collider
            GameObject newCollider = new GameObject("Collider");
            newCollider.AddComponent<BoxCollider>();
            newCollider.transform.parent = newPieceObject.transform;
            newCollider.transform.position = new Vector3(
                    pieceObject.transform.position.x,
                    pieceObject.transform.position.y + gridUnitSize/2,
                    pieceObject.transform.position.z);
            newCollider.GetComponent<BoxCollider>().size = new Vector3(gridUnitSize, gridUnitSize, gridUnitSize);
        }

        // Perform a raycast and get the target position
        private Vector3Int GetTargetGridPosition(float maxHitDisance, float maxFloatingDistance, Vector3 forward, Vector3 origin, bool stack, out bool didHit, out GameObject hitObject)
        {
            RaycastHit hit;
            // The normal vector of the point of the collider hit by the raycast.
            Vector3Int normal;
            Vector3Int gridPositionHit;

            if (Physics.Raycast(origin, forward, out hit, maxHitDisance))
            {
                Debug.DrawRay(origin, forward * hit.distance, Color.red);
                // The collider is a cube, so we round each component of the normal vector to integers.
                normal = new Vector3Int(Mathf.RoundToInt(hit.normal.x), Mathf.RoundToInt(hit.normal.y), Mathf.RoundToInt(hit.normal.z));

                // Get the position of the piece the raycast hit.
                Vector3 positionHit = hit.transform.parent.transform.position;
                gridPositionHit = GetGridPositionFromWorldPosition(positionHit);

                // Return values if the raycast hit
                didHit = true;
                hitObject = hit.transform.parent.gameObject;
                // If we aren't stacking, add the position of the raycast hit to the normal vector.
                // This is used to find the position adjacent to the piece that was hit. It helps avoid stacking the pieces when stacking is disabled.
                return stack ? gridPositionHit : gridPositionHit + normal;
            }
            else
            {
                // Return values if the raycast missed
                didHit = false;
                hitObject = null;
                return GetGridPositionFromWorldPosition(origin + (maxFloatingDistance * forward));
            }
        }

        // Some overloads of GetTargetGridPosition. I don't think they are actually used, but maybe they will come in useful
        private Vector3Int GetTargetGridPosition(float maxDistance, Vector3 forward, Vector3 origin, bool stack, out bool didHit, out GameObject hitObject)
        {
            return GetTargetGridPosition(maxDistance, maxDistance, forward, origin, stack, out didHit, out hitObject);
        }

        private Vector3Int GetTargetGridPosition(float maxDistance, Vector3 forward, Vector3 origin, bool stack, out bool didHit)
        {
            GameObject dummy;
            return GetTargetGridPosition(maxDistance, maxDistance, forward, origin, stack, out didHit, out dummy);
        }

        private Vector3Int GetTargetGridPosition(float maxHitDisance, float maxFloatingDistance, Vector3 forward, Vector3 origin, bool stack)
        {
            bool dummy1;
            GameObject dummy2;
            return GetTargetGridPosition(maxHitDisance, maxFloatingDistance, forward, origin, stack, out dummy1, out dummy2);
        }

        // Converts a world position to its position on the grid based on the grid unit size.
        private Vector3Int GetGridPositionFromWorldPosition(Vector3 position)
        {
            return new Vector3Int(
                    Mathf.RoundToInt (position.x / gridUnitSize),
                    Mathf.RoundToInt (position.y / gridUnitSize),
                    Mathf.RoundToInt (position.z / gridUnitSize));
        }

        // Converts a grid position to its world position. The exact opposite of GetGridPositionFromWorldPosition.
        private Vector3 WorldPositionFromGridPosition(Vector3Int gridPosition)
        {
            return new Vector3(gridPosition.x * gridUnitSize, gridPosition.y * gridUnitSize, gridPosition.z * gridUnitSize);
        }
    }
}
