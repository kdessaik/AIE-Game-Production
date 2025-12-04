using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager Instance { get; private set; }

    [System.Serializable]
    public class PrefabPool
    {
        public GameObject prefab;
        public int initialSize = 10;
        [HideInInspector] public Queue<GameObject> queue = new Queue<GameObject>();
    }

    public PrefabPool[] pools;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        foreach (var p in pools)
        {
            for (int i = 0; i < p.initialSize; i++)
            {
                var go = Instantiate(p.prefab);
                go.SetActive(false);
                p.queue.Enqueue(go);
            }
        }
    }

    public GameObject GetFromPool(int poolIndex)
    {
        if (poolIndex < 0 || poolIndex >= pools.Length) return null;
        var p = pools[poolIndex];
        if (p.queue.Count == 0)
        {
            var go = Instantiate(p.prefab);
            go.SetActive(false);
            p.queue.Enqueue(go);
        }
        var obj = p.queue.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj == null) return;
        // find which pool it belongs to by prefab name match (simple)
        for (int i = 0; i < pools.Length; i++)
        {
            if (obj.name.StartsWith(pools[i].prefab.name))
            {
                obj.SetActive(false);
                pools[i].queue.Enqueue(obj);
                return;
            }
        }
        // not found -> destroy
        Destroy(obj);
    }
}
