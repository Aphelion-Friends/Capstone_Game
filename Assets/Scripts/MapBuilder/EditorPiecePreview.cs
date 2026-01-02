using UnityEngine;

namespace MapBuilder
{
    public class EditorPiecePreview : MonoBehaviour
    {
        MapPiece piece;
        GameObject piecePrefab;
        GameObject pieceObject;
        bool pieceInstantiated = false;

        [SerializeField] private float floatingReachDistance = 10f;
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
            gridUnitSize = MapEditor.Instance.map.gridUnitSize;
        }

        void Update()
        {
            if (!MapEditor.Instance.assetsLoaded)
                return;

            if (!_prefabNameSet)
                return;

            if (!pieceInstantiated)
            {
                if (pieceObject is not null)
                    Destroy(pieceObject);

                piecePrefab = MapEditor.Instance.mapPiecePrefabs[_prefabName];
                piece = new MapPiece(new Vector3Int(0, 0, 0), new Piece(_prefabName), 0);
                pieceObject = Instantiate(piecePrefab);
                pieceInstantiated = true;
            }

            if (MapEditorInputManager.Instance.menuOpen)
                return;

            if (targetedPieceObject is not null)
                targetedPieceObject.SetActive(true);

            bool colliderHit;
            Vector3Int gridTargetPosition;
            float currentReachDistance = 0f;
            bool stack = false;
            bool hidePreview = false;
            bool canPlacePiece = false;
            bool canRemovePiece = false;
            bool hidePreviewOnMiss = false;
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
            gridTargetPosition = GetTargetGridPosition(
                    maxDistance:currentReachDistance,
                    forward:cameraScript.playerCamera.transform.forward,
                    origin:cameraScript.playerCamera.transform.position,
                    stack:stack,
                    out colliderHit,
                    out targetedPieceObject);

            if (force)
            {
                gridTargetPosition = GetGridPositionFromWorldPosition(
                        cameraScript.playerCamera.transform.position +
                        (currentReachDistance * cameraScript.playerCamera.transform.forward));
            }

            if (!colliderHit && hidePreviewOnMiss)
            {
                hidePreview = true;
            }

            if (canRemovePiece && colliderHit)
            {
                targetedPieceObject.SetActive(false);
            }

            pieceObject.SetActive(!hidePreview);
            piece.location = gridTargetPosition;
            pieceObject.transform.position = WorldPositionFromGridPosition(gridTargetPosition);
        

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

        private void AddPiece(MapPiece mapPiece, GameObject pieceObject)
        {
            MapEditor.Instance.map.AddMapPiece(piece);
            GameObject newPieceObject = Instantiate(pieceObject);

            GameObject newCollider = new GameObject("Collider");
            newCollider.AddComponent<BoxCollider>();
            newCollider.transform.parent = newPieceObject.transform;
            newCollider.transform.position = new Vector3(
                    pieceObject.transform.position.x,
                    pieceObject.transform.position.y + gridUnitSize/2,
                    pieceObject.transform.position.z);
            newCollider.GetComponent<BoxCollider>().size = new Vector3(gridUnitSize, gridUnitSize, gridUnitSize);
        }

        private Vector3Int GetTargetGridPosition(float maxHitDisance, float maxFloatingDistance, Vector3 forward, Vector3 origin, bool stack, out bool didHit, out GameObject hitObject)
        {
            RaycastHit hit;
            Vector3Int normal;
            Vector3Int gridPositionHit;

            if (Physics.Raycast(origin, forward, out hit, maxHitDisance))
            {
                Debug.DrawRay(origin, forward * hit.distance, Color.red);
                normal = new Vector3Int(Mathf.RoundToInt(hit.normal.x), Mathf.RoundToInt(hit.normal.y), Mathf.RoundToInt(hit.normal.z));
                Vector3 positionHit = hit.transform.parent.transform.position;
                gridPositionHit = GetGridPositionFromWorldPosition(positionHit);

                didHit = true;
                hitObject = hit.transform.parent.gameObject;
                return stack ? gridPositionHit : gridPositionHit + normal;
            }
            else
            {
                didHit = false;
                hitObject = null;
                return GetGridPositionFromWorldPosition(origin + (maxFloatingDistance * forward));
            }
        }

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

        private Vector3Int GetGridPositionFromWorldPosition(Vector3 position)
        {
            return new Vector3Int(
                    Mathf.RoundToInt (position.x / gridUnitSize),
                    Mathf.RoundToInt (position.y / gridUnitSize),
                    Mathf.RoundToInt (position.z / gridUnitSize));
        }

        private Vector3 WorldPositionFromGridPosition(Vector3Int gridPosition)
        {
            return new Vector3(gridPosition.x * gridUnitSize, gridPosition.y * gridUnitSize, gridPosition.z * gridUnitSize);
        }
    }
}
