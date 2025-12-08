using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [System.Serializable]
    public class PrefabPool
    {
        public string name;
        public GameObject prefab;
        public int initialSize = 10;
        [HideInInspector] public Queue<GameObject> queue;
    }

    public PrefabPool[] pools;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        foreach (var p in pools)
        {
            p.queue = new Queue<GameObject>();
            for (int i = 0; i < p.initialSize; i++)
            {
                var go = Instantiate(p.prefab);
                go.name = p.prefab.name; // Normalize name for ReturnToPool matching
                go.SetActive(false);
                p.queue.Enqueue(go);
            }
        }
    }

    // Get an object from pool index
    public GameObject GetFromPool(int poolIndex)
    {
        if (poolIndex < 0 || poolIndex >= pools.Length) return null;
        var p = pools[poolIndex];

        if (p.queue.Count == 0)
        {
            // expand pool on demand
            var go = Instantiate(p.prefab);
            go.name = p.prefab.name;
            go.SetActive(false);
            p.queue.Enqueue(go);
        }

        var obj = p.queue.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    // Return object to its pool (by prefab name match)
    public void ReturnToPool(GameObject obj)
    {
        if (obj == null) return;

        // normalize name (handle "(Clone)")
        string objName = obj.name;
        if (objName.EndsWith("(Clone)")) objName = objName.Replace("(Clone)", "").Trim();

        for (int i = 0; i < pools.Length; i++)
        {
            if (pools[i].prefab.name == objName)
            {
                obj.SetActive(false);
                pools[i].queue.Enqueue(obj);
                return;
            }
        }

        // not found -> destroy (fallback)
        Destroy(obj);
    }
}
