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
        private Dictionary<string, AsyncOperationHandle<GameObject>> _operationDictionary;
        public Dictionary<string, AsyncOperationHandle<GameObject>> operationDictionary { get { return _operationDictionary; } }
        // An event that gets triggered when the addressables are loaded
        public UnityEvent Ready;

        public void LoadAssets(List<string> keys)
        {
            StartCoroutine(LoadAndAssociateResultWithKey(keys));
        }

        void Awake()
        {
            if (Ready == null)
                Ready = new UnityEvent();
        }

        // IDK how this works, I just copy pasted it from the Unity docs
        // From: https://docs.unity3d.com/Packages/com.unity.addressables@1.19/manual/LoadingAddressableAssets.html#correlating-loaded-assets-to-their-keys
        IEnumerator LoadAndAssociateResultWithKey(IList<string> keys) {

            if (_operationDictionary == null)
                _operationDictionary = new Dictionary<string, AsyncOperationHandle<GameObject>>();

            AsyncOperationHandle<IList<IResourceLocation>> locations
                = Addressables.LoadResourceLocationsAsync(keys,
                    Addressables.MergeMode.Union, typeof(GameObject));

            yield return locations;

            var loadOps = new List<AsyncOperationHandle>(locations.Result.Count);

            foreach (IResourceLocation location in locations.Result) {
                AsyncOperationHandle<GameObject> handle =
                    Addressables.LoadAssetAsync<GameObject>(location);
                handle.Completed += obj => _operationDictionary.Add(location.PrimaryKey, obj);
                loadOps.Add(handle);
            }

            yield return Addressables.ResourceManager.CreateGenericGroupOperation(loadOps, true);

            Ready.Invoke();
        }

        private void OnDestroy()
        {
            foreach (var item in _operationDictionary) {
                Addressables.Release(item.Value);
            }
        }
    }
}
