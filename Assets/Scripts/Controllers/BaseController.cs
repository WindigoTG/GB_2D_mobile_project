using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

public class BaseController : IDisposable
{
    private List<BaseController> _baseControllers = new List<BaseController>();
    private List<GameObject> _gameObjects = new List<GameObject>();
    private List<AsyncOperationHandle<GameObject>> _addressablePrefabs = new List<AsyncOperationHandle<GameObject>>();
    private bool _isDisposed;
    Animator animator;
    
    public void Dispose()
    {
        if (_isDisposed) 
            return;
        
        _isDisposed = true;
            
        foreach (var baseController in _baseControllers)
            baseController?.Dispose();
                
        _baseControllers.Clear();
        
        foreach (var cachedGameObject in _gameObjects)
            Object.Destroy(cachedGameObject);
                
        _gameObjects.Clear();

        foreach (var addressablePrefab in _addressablePrefabs)
            Addressables.ReleaseInstance(addressablePrefab);

        _addressablePrefabs.Clear();

        OnDispose();
    }

    protected void AddController(BaseController baseController)
    {
        _baseControllers.Add(baseController);
    }

    protected void AddGameObject(GameObject gameObject)
    {
        _gameObjects.Add(gameObject);
    }

    protected async void CreateAddressablesPrefab<T>(AssetReference prefabToLoad, Transform spawnParent, Action<T> callback)
    {
        var addressablePrefab = Addressables.InstantiateAsync(prefabToLoad, spawnParent);
        _addressablePrefabs.Add(addressablePrefab);

        await addressablePrefab.Task;
        
        callback.Invoke(addressablePrefab.Result.GetComponent<T>());
    }

    protected virtual void OnDispose()
    {
    }
}
