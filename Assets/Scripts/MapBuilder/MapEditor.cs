using UnityEngine;
using System.Collections.Generic;

namespace MapBuilder
{
    // A singleton class that is responsible for storing the Map object and all the prefabs used for editing the map.
    public class MapEditor : MonoBehaviour
    {
        private static MapEditor _instance;
        public static MapEditor Instance { get { return _instance; } }

        private AddressablesLoader addressablesLoader;

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
            addressablesLoader = gameObject.AddComponent<AddressablesLoader>();
            addressablesLoader.Ready.AddListener(OnAssetsReady);
            addressablesLoader.LoadAssets(_keys);
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
            foreach (var op in addressablesLoader.operationDictionary)
            {
                _mapPiecePrefabs[op.Key] = op.Value.Result;
            }

            _assetsLoaded = true;
        }

        // When the game stops, save the map to a file
        private void OnDestroy()
        {
            MapFileStorage mapFileStorage = new MapFileStorage();
            mapFileStorage.WriteMapToFile(_map, _mapName);
        }
    }
}
