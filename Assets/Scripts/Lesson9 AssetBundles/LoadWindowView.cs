using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

namespace AssetBundles
{
    public class LoadWindowView : AssetBundleViewBase
    {
        [SerializeField]
        private Button _loadAsssetsButton;

        [SerializeField]
        private AssetReference _loadPrefab;

        [SerializeField]
        private RectTransform _spawnParent;

        [SerializeField]
        private Button _spawnAssetsButton;

        private List<AsyncOperationHandle<GameObject>> _addressablePrefabs = new List<AsyncOperationHandle<GameObject>>();

        private void Start()
        {
            _loadAsssetsButton.onClick.AddListener(LoadAsset);
            _spawnAssetsButton.onClick.AddListener(CreateAddressablesPrefab);
        }

        private void OnDestroy()
        {
            _loadAsssetsButton.onClick.RemoveAllListeners();

            foreach (var addressablePrefab in _addressablePrefabs)
                Addressables.ReleaseInstance(addressablePrefab);

            _addressablePrefabs.Clear();
        }

        private void LoadAsset()
        {
            _loadAsssetsButton.interactable = false;

            StartCoroutine(DownloadAndSetAssetBundle());
        }

        private void CreateAddressablesPrefab()
        {
            var addressablePrefab = Addressables.InstantiateAsync(_loadPrefab, _spawnParent);
            _addressablePrefabs.Add(addressablePrefab);
        }

    }
}