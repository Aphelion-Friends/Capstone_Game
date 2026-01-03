using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace MapBuilder
{
    // A singleton class that is responsible for storing the Map object and all the prefabs used for editing the map.
    public class MapEditor : MonoBehaviour
    {
        private static MapEditor _instance;
        public static MapEditor Instance { get { return _instance; } }

        [SerializeField] private string _mapName;

        private Map _map;
        public Map map { get => _map; }

        // If you want a prefab to appear in the selection menu, you have to add it to this list.
        // I don't know how to get the list of addressables names
        private List<string> _keys = new List<string> {
            "wall",
            "ceiling",
            "floor"
        };
        public List<string> keys { get { return _keys; } }

        private Dictionary<string, AsyncOperationHandle<GameObject>> operationDictionary;
        public UnityEvent Ready;

        private Dictionary<string, GameObject> _mapPiecePrefabs;
        public Dictionary<string, GameObject> mapPiecePrefabs { get { return _mapPiecePrefabs; } }

        private bool _assetsLoaded = false;
        public bool assetsLoaded { get { return _assetsLoaded; } }

        void Awake()
        {
            _instance = this;
        }


        void Start()
        {
            Ready.AddListener(OnAssetsReady);
            StartCoroutine(LoadAndAssociateResultWithKey(_keys));
            LoadMap(_mapName);
        }

        public void LoadMap(string mapName)
        {
            _mapName = mapName;

            // Get the map from its json file
            MapFileStorage mapFileStorage = new MapFileStorage();
            _map = mapFileStorage.ReadMapFromFile(mapName);

            // Spawn in a MapInitializer to handle loading the map
            MapInitializer mapInitializer = gameObject.AddComponent<MapInitializer>();
            mapInitializer.Initialize(_map, editMode:true);
        }

        private void OnAssetsReady()
        {
            _mapPiecePrefabs = new Dictionary<string, GameObject>();
            foreach (var op in operationDictionary)
            {
                Debug.Log($"Key: {op.Key}");
                _mapPiecePrefabs[op.Key] = op.Value.Result;
            }

            _assetsLoaded = true;
        }

        // IDK how this works, I just copy pasted it from the Unity docs
        // From: https://docs.unity3d.com/Packages/com.unity.addressables@1.19/manual/LoadingAddressableAssets.html#correlating-loaded-assets-to-their-keys
        IEnumerator LoadAndAssociateResultWithKey(IList<string> keys) {
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

            Ready.Invoke();
        }

        // When the game stops, free the addressables and save the map to a file
        private void OnDestroy()
        {
            foreach (var item in operationDictionary) {
                Addressables.Release(item.Value);
            }
            MapFileStorage mapFileStorage = new MapFileStorage();
            mapFileStorage.WriteMapToFile(_map, _mapName);
        }
    }
}
