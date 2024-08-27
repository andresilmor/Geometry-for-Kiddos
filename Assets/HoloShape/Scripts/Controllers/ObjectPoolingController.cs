using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public static class ObjectPoolingController
{
    public static Dictionary<string, List<GameObject>> ObjectPool = new Dictionary<string, List<GameObject>>();

    static GameObject _pool = null;

    public static void AddToPool(string key, GameObject obj) {
        if (_pool == null)
            _pool =  GameObject.FindGameObjectWithTag("ObjectPool");

        if (ObjectPool.ContainsKey(key))
            ObjectPool.Add(key, new List<GameObject>());
        
        ObjectPool[key].Add(obj);   
   
    }   

    public static Task  RemoveFromPool(string key) { 
        foreach (GameObject obj in ObjectPool[key])
            Object.Destroy(obj);   
        
        ObjectPool.Remove(key); 

        return Task.CompletedTask;
    
    }

    public static GameObject RetrieveFromPool(string key) {
        if (!ObjectPool.ContainsKey(key) || ObjectPool[key].Count == 0)
            return null;
        
        return ObjectPool[key][0];

    }

}
