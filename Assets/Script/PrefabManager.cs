using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics; // quaternion을 사용하기 위해 필요
using UnityEngine.Pool;

public class PrefabManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PrefabManager Instance { get; private set; }

    // 프리팹들을 담을 딕셔너리 (Inspector에서 수동 할당용)
    [System.Serializable]
    public class PoolSetup
    {
        public GameObject prefab;
        public string prefabName;
        public int defaultCapacity = 10;
        public int maxSize = 100;
    }

    public PoolSetup[] poolSetups;
    private Dictionary<GameObject, ObjectPool<GameObject>> pools;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않도록
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        pools = new Dictionary<GameObject, ObjectPool<GameObject>>();
        foreach (var setup in poolSetups)
        {
            var pool = new ObjectPool<GameObject>(
                () =>
                {
                    GameObject obj = Instantiate(setup.prefab);
                    obj.name = setup.prefabName;
                    return obj;
                },
                obj => obj.SetActive(true),
                obj => { obj.SetActive(false); obj.transform.SetParent(this.transform); },
                Destroy,
                true,
                setup.defaultCapacity,
                setup.maxSize
            );
            pools.Add(setup.prefab, pool);

        }
    }

    public PoolSetup GetPrefabByName(string nameToFind)
    {
        // poolSetups 배열이 null이거나 비어 있는지 확인합니다.
        if (poolSetups == null || poolSetups.Length == 0)
        {
            Debug.LogWarning("PoolSetups 배열이 초기화되지 않았거나 비어 있습니다.");
            return null;
        }

        // 배열의 각 PoolSetup을 반복합니다.
        foreach (PoolSetup setup in poolSetups)
        {
            // prefabName을 비교합니다 (대소문자 구분).
            if (setup.prefabName == nameToFind)
            {
                return setup; // 이름이 일치하면 GameObject를 반환합니다.
            }
        }

        // 모든 항목을 확인한 후에도 일치하는 prefabName을 찾지 못한 경우
        Debug.LogWarning($"'{nameToFind}' 이름의 프리팹을 poolSetups에서 찾을 수 없습니다.");
        return null;
    }

    public GameObject Get(string name, Vector3 position, Quaternion rotation)
    {
        if (pools.TryGetValue(GetPrefabByName(name).prefab, out var pool))
        {
            GameObject obj = pool.Get();
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.SetParent(this.transform);
            return obj;
        }
        Debug.LogError($"Pool for prefab {name} not found!");
        return null;
    }
    
     public void Release(GameObject obj)
    {
        if (pools.TryGetValue(GetPrefabByName(obj.name).prefab, out var pool))
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
