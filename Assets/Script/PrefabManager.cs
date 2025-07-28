using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics; // quaternion을 사용하기 위해 필요

public class PrefabManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PrefabManager Instance { get; private set; }

    // 프리팹들을 담을 딕셔너리 (Inspector에서 수동 할당용)
    [System.Serializable]
    public class PrefabEntry
    {
        public string prefabName;
        public GameObject prefabObject;
        public int initialPoolSize = 5; // 초기 풀 크기 설정
    }

    public List<PrefabEntry> prefabList = new List<PrefabEntry>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();
    // 오브젝트 풀을 저장할 딕셔너리: 프리팹 이름 -> 해당 프리팹의 풀 리스트
    private Dictionary<string, List<GameObject>> objectPool = new Dictionary<string, List<GameObject>>();

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

        // 리스트의 프리팹들을 딕셔너리에 추가하여 빠른 접근 가능하게 함
        foreach (PrefabEntry entry in prefabList)
        {
            if (!prefabDictionary.ContainsKey(entry.prefabName))
            {
                prefabDictionary.Add(entry.prefabName, entry.prefabObject);
                InitializePool(entry.prefabName, entry.prefabObject, entry.initialPoolSize); // 풀 초기화
            }
            else
            {
                Debug.LogWarning($"PrefabManager: Duplicate prefab name found: {entry.prefabName}. Only the first one will be used.");
            }
        }
    }

    /// <summary>
    /// 지정된 프리팹에 대한 오브젝트 풀을 초기화합니다.
    /// </summary>
    /// <param name="prefabName">프리팹 이름</param>
    /// <param name="prefabObject">프리팹 오브젝트</param>
    /// <param name="initialSize">초기 풀 크기</param>
    private void InitializePool(string prefabName, GameObject prefabObject, int initialSize)
    {
        if (!objectPool.ContainsKey(prefabName))
        {
            objectPool.Add(prefabName, new List<GameObject>());
        }

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = Instantiate(prefabObject);
            obj.SetActive(false); // 비활성화 상태로 풀에 추가
            obj.transform.SetParent(this.transform); // PrefabManager 아래에 자식으로 두어 계층 구조를 깔끔하게 유지
            objectPool[prefabName].Add(obj);
        }
        Debug.Log($"PrefabManager: Initialized pool for '{prefabName}' with {initialSize} objects.");
    }

    /// <summary>
    /// 지정된 이름의 프리팹을 풀에서 가져오거나 새로 생성하여 인스턴스화합니다.
    /// </summary>
    /// <param name="prefabName">가져올 프리팹의 이름</param>
    /// <param name="position">새로 생성될 오브젝트의 위치</param>
    /// <param name="rotation">새로 생성될 오브젝트의 회전</param>
    /// <param name="parent">새로 생성될 오브젝트의 부모 Transform (선택 사항)</param>
    /// <returns>생성된 GameObject 또는 null (프리팹을 찾지 못했을 경우)</returns>
    public GameObject InstantiatePrefab(string prefabName, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        if (!prefabDictionary.ContainsKey(prefabName))
        {
            Debug.LogError($"PrefabManager: Prefab with name '{prefabName}' not found in dictionary. Consider adding it in the inspector or loading it dynamically.");
            return null;
        }

        // 풀에서 비활성화된 오브젝트를 찾습니다.
        if (objectPool.TryGetValue(prefabName, out List<GameObject> pool))
        {
            foreach (GameObject obj in pool)
            {
                if (!obj.activeInHierarchy) // 비활성화된 오브젝트가 있다면
                {
                    obj.transform.SetPositionAndRotation(position, rotation);
                    obj.transform.SetParent(parent);
                    obj.SetActive(true);
                    return obj;
                }
            }
        }
        else
        {
            // 아직 풀이 초기화되지 않은 프리팹인 경우, 새 풀을 생성합니다.
            // objectPool.Add(prefabName, new List<GameObject>());
        }

        // 비활성화된 오브젝트가 없으면 새로 생성합니다.
        GameObject newObject = Instantiate(prefabDictionary[prefabName], position, rotation, parent);
        newObject.name = prefabName; // 이름 설정
        objectPool[prefabName].Add(newObject); // 새로 생성된 오브젝트를 풀에 추가
        Debug.Log($"PrefabManager: Instantiated new object for '{prefabName}' (pool expanded).");
        return newObject;
    }

    /// <summary>
    /// 지정된 이름의 프리팹을 풀에서 가져오거나 새로 생성하여 인스턴스화합니다.
    /// (위치와 회전은 기본값으로 설정)
    /// </summary>
    /// <param name="prefabName">가져올 프리팹의 이름</param>
    /// <param name="parent">새로 생성될 오브젝트의 부모 Transform (선택 사항)</param>
    /// <returns>생성된 GameObject 또는 null (프리팹을 찾지 못했을 경우)</returns>
    public GameObject InstantiatePrefab(string prefabName,Transform trans ,Transform parent = null)
    {
        // 기본 위치와 회전으로 오버로드된 함수를 호출합니다.
        return InstantiatePrefab(prefabName, trans.position, trans.rotation,parent);
    }

    /// <summary>
    /// 사용이 끝난 오브젝트를 풀로 반환하여 비활성화합니다.
    /// </summary>
    /// <param name="obj">풀로 반환할 GameObject</param>
    public void ReturnPrefab(GameObject obj)
    {
        if (obj == null) return;

        // 오브젝트의 원래 프리팹 이름을 사용하여 풀을 찾습니다.
        // InstantiatePrefab에서 name을 prefabName으로 설정했기 때문에 가능합니다.
        string prefabName = obj.name; 

        if (objectPool.ContainsKey(prefabName))
        {
            obj.SetActive(false);
            obj.transform.SetParent(this.transform); // PrefabManager 아래로 다시 이동
            // 이미 풀에 있는 오브젝트인지 확인 (중복 추가 방지)
            if (!objectPool[prefabName].Contains(obj))
            {
                objectPool[prefabName].Add(obj);
            }
        }
        else
        {
            // 풀에 없는 오브젝트이거나, 풀링되지 않은 오브젝트일 경우 그냥 파괴합니다.
            Debug.LogWarning($"PrefabManager: Attempted to return an object '{obj.name}' to a pool that does not exist or was not managed by this pool. Destroying object.");
            Destroy(obj);
        }
    }

    /// <summary>
    /// 지정된 이름의 프리팹을 로드합니다. (인스턴스화하지 않음)
    /// </summary>
    /// <param name="prefabName">로드할 프리팹의 이름</param>
    /// <returns>GameObject 프리팹 또는 null (프리팹을 찾지 못했을 경우)</returns>
    public GameObject GetPrefab(string prefabName)
    {
        if (prefabDictionary.TryGetValue(prefabName, out GameObject prefab))
        {
            return prefab;
        }
        else
        {
            Debug.LogError($"PrefabManager: Prefab with name '{prefabName}' not found in dictionary. Consider adding it in the inspector or loading it dynamically.");
            return null;
        }
    }

    // Resources 폴더에서 프리팹을 동적으로 로드하는 예시 (필요시 사용)
    // 이 함수로 로드된 프리팹도 풀링 시스템에 통합됩니다.
    public GameObject LoadPrefabFromResources(string path, int initialPoolSize = 5)
    {
        GameObject loadedPrefab = Resources.Load<GameObject>(path);
        if (loadedPrefab != null)
        {
            if (!prefabDictionary.ContainsKey(loadedPrefab.name))
            {
                prefabDictionary.Add(loadedPrefab.name, loadedPrefab);
                InitializePool(loadedPrefab.name, loadedPrefab, initialPoolSize); // 동적으로 로드된 프리팹도 풀 초기화
                Debug.Log($"PrefabManager: Dynamically loaded and added '{loadedPrefab.name}' to dictionary and initialized pool.");
            }
            return loadedPrefab;
        }
        else
        {
            Debug.LogError($"PrefabManager: Failed to load prefab from Resources path: '{path}'. Check the path and file type.");
            return null;
        }
    }
}
