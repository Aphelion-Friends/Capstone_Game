using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace MapBuilder
{
    // This class is responsible for loading the map into the game based on a Map object
    public class MapInitializer : MonoBehaviour
    {
        [SerializeField] private string mapName;
        // If you attach this script to a GameObject and you want to load the map when the game starts, set this to true
        [SerializeField] private bool autoInitialize = false;

        private Map map;

        // Maps addressable names to their assets
        private Dictionary<string, AsyncOperationHandle<GameObject>> operationDictionary;
        // List of all addressable names
        private List<string> keys;
        // Maps the addressable name of a map piece to its MapPiece object
        private Dictionary<string, MapPiece> mapPieceDictionary;
        // An event that gets triggered when the addressables are loaded
        public UnityEvent Ready;

        // If _editMode is true, a box collider will be created for each map piece
        private bool _editMode = false;

        private System.Diagnostics.Stopwatch addressablesLoadStopwatch;
        private System.Diagnostics.Stopwatch mapLoadStopwatch;

        public void Initialize(Map newMap, bool editMode)
        {
            mapLoadStopwatch = System.Diagnostics.Stopwatch.StartNew();

            _editMode = editMode;
            map = newMap;
            keys = GetAllMapPrefabNames();
            Debug.Log($"Num keys: {keys.Count}");
            mapPieceDictionary = GetMapPieceDictionary();
            StartCoroutine(LoadAndAssociateResultWithKey(keys));
        }

        public void Initialize(Map newMap)
        {
            this.Initialize(newMap, false);
        }

        void Awake()
        {
            if (Ready == null)
                Ready = new UnityEvent();

            Ready.AddListener(OnAssetsReady);

            if (autoInitialize)
            {
                MapFileStorage mapFileStorage = new MapFileStorage();
                Map mapFromFile = mapFileStorage.ReadMapFromFile(mapName);
                Initialize(mapFromFile);
            }
        }

        // This function is called after the addressables finish loading
        private void OnAssetsReady()
        {
            foreach (var mapPiece in map.pieces)
            {
                // Create all the map piece GameObjects and set their position and rotation
                Vector3 position = new Vector3(mapPiece.location.x * map.gridUnitSize, mapPiece.location.y * map.gridUnitSize, mapPiece.location.z * map.gridUnitSize);
                Quaternion rotation = new Quaternion();
                rotation.eulerAngles = new Vector3(0, mapPiece.orientation * 90, 0);

                GameObject newPiece = Instantiate(operationDictionary[mapPiece.piece.prefabName].Result, position, rotation);

                // Create the box colliders
                if (_editMode)
                {
                    GameObject newCollider = new GameObject("Collider");
                    newCollider.AddComponent<BoxCollider>();
                    newCollider.transform.parent = newPiece.transform;
                    newCollider.transform.position = new Vector3(
                            newPiece.transform.position.x,
                            newPiece.transform.position.y + map.gridUnitSize/2,
                            newPiece.transform.position.z);
                    newCollider.GetComponent<BoxCollider>().size = new Vector3(map.gridUnitSize, map.gridUnitSize, map.gridUnitSize);
                }
            }


            mapLoadStopwatch.Stop();
            Debug.Log($"Total map loading time: {mapLoadStopwatch.ElapsedMilliseconds}ms");
        }
    

        // From: https://docs.unity3d.com/Packages/com.unity.addressables@1.19/manual/LoadingAddressableAssets.html#correlating-loaded-assets-to-their-keys
        IEnumerator LoadAndAssociateResultWithKey(IList<string> keys) {
            addressablesLoadStopwatch = System.Diagnostics.Stopwatch.StartNew();

            if (operationDictionary == null)
                operationDictionary = new Dictionary<string, AsyncOperationHandle<GameObject>>();

            AsyncOperationHandle<IList<IResourceLocation>> locations
                = Addressables.LoadResourceLocationsAsync(keys,
                    Addressables.MergeMode.Union, typeof(GameObject));

            yield return locations;

            var loadOps = new List<AsyncOperationHandle>(locations.Result.Count);

            foreach (IResourceLocation location in locations.Result) {
                AsyncOperationHandle<GameObject> handle =
                    Addressables.LoadAssetAsync<GameObject>(location);
                handle.Completed += obj => operationDictionary.Add(location.PrimaryKey, obj);
                loadOps.Add(handle);
            }

            yield return Addressables.ResourceManager.CreateGenericGroupOperation(loadOps, true);

            addressablesLoadStopwatch.Stop();
            Debug.Log($"Addressables load time: {addressablesLoadStopwatch.ElapsedMilliseconds}ms");
            Ready.Invoke();
        }

        private List<string> GetAllMapPrefabNames()
        {
            List<string> listOfNames = new List<string>();

            foreach (MapPiece mapPiece in map.pieces)
            {
                listOfNames.Add(mapPiece.piece.prefabName);
            }

            return listOfNames;
        }

        private Dictionary<string, MapPiece> GetMapPieceDictionary()
        {
            Dictionary<string, MapPiece> mapPieceDictionary = new Dictionary<string, MapPiece>();

            foreach (MapPiece mapPiece in map.pieces)
            {
                mapPieceDictionary[mapPiece.piece.prefabName] = mapPiece;
            }

            return mapPieceDictionary;
        }
        private void OnDestroy()
        {
            foreach (var item in operationDictionary) {
                Addressables.Release(item.Value);
            }
        }
    }
}
