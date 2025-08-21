using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics; // quaternion을 사용하기 위해 필요
using UnityEngine.Pool;
using System;

public class PrefabManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PrefabManager Instance { get; private set; }

    public GameObject[] readyPrefabs;

    private Dictionary<String, ObjectPool<GameObject>> pools;

    public int maxSize;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않도록
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        pools = new Dictionary<String, ObjectPool<GameObject>>();
        foreach (var setup in readyPrefabs)
        {
            var pool = new ObjectPool<GameObject>(
                () =>
                {
                    GameObject obj = Instantiate(setup);
                    obj.name = setup.name;
                    return obj;
                },
                obj => obj.SetActive(true),
                obj => { obj.SetActive(false); obj.transform.SetParent(this.transform); },
                Destroy,
                true,
                0,
                maxSize
            );
            pools.Add(setup.name, pool);

        }
    }

    public GameObject Get(string name, Vector3 position, Quaternion rotation, float scale = 1)
    {
        if (pools.TryGetValue(name, out var pool))
        {
            GameObject obj = pool.Get();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            
            obj.transform.localScale.Set(scale,scale,scale);
            obj.transform.SetParent(this.transform);
            
            return obj;
        }
        Debug.LogError($"Pool for prefab {name} not found!");
        return null;
    }
    
     public void Release(GameObject obj)
    {
        if (pools.TryGetValue(obj.name, out var pool))
        {
            pool.Release(obj);
        }
        else
        {
            Debug.LogWarning($"Trying to release an object from an unknown pool: {obj.name}. Destroying it instead.");
            Destroy(obj); // 풀이 없으면 그냥 파괴
        }
    }

    
}
