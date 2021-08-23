using UnityEngine;

public static class ResourceLoader
{
    public static GameObject LoadPrefab(string path)
    {
        return Resources.Load<GameObject>(path);
    }
} 
