using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.ResourceLocations;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace MapBuilder
{
    // This class is responsible for loading several prefabs at once using addressables
    public class AddressablesLoader : MonoBehaviour
    {
        // Maps addressable names to their assets
        private Dictionary<string, GameObject> _gameObjectDictionary;
        public Dictionary<string, GameObject> gameObjectDictionary { get { return _gameObjectDictionary; } }
        private Dictionary<string, AsyncOperationHandle<GameObject>> operationDictionary;
        // An event that gets triggered when the addressables are loaded
        public UnityEvent Ready;

        void LoadAssets(List<string> keys)
        {
            StartCoroutine(LoadAndAssociateResultWithKey(keys));
        }

        void Awake()
        {
            if (Ready == null)
                Ready = new UnityEvent();

            Ready.AddListener(OnAssetsReady);
        }

        private void OnAssetsReady()
        {
            foreach(var op in operationDictionary)
            {
                _gameObjectDictionary[op.Key] = op.Value.Result;
            }
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
    }
}
