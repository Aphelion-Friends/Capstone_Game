using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceLocations;

namespace MapBuilder
{
    public class MapEditor : MonoBehaviour
    {
        private static MapEditor _instance;
        public static MapEditor Instance { get { return _instance; } }

        [SerializeField] private string _mapName;

        private Map _map;
        public Map map { get => _map; }

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

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            Ready.AddListener(OnAssetsReady);
            StartCoroutine(LoadAndAssociateResultWithKey(_keys));
            LoadMap(_mapName);
        }

        public void LoadMap(string mapName)
        {
            _mapName = mapName;

            MapFileStorage mapFileStorage = new MapFileStorage();
            _map = mapFileStorage.ReadMapFromFile(mapName);

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
