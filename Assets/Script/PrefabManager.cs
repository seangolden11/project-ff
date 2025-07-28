using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class PrefabManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static PrefabManager Instance { get; private set; }

    // 프리팹들을 담을 딕셔너리
    // Inspector에서 수동으로 할당하거나, Resources.Load 등을 통해 동적으로 로드 가능
    [System.Serializable]
    public class PrefabEntry
    {
        public string prefabName;
        public GameObject prefabObject;
    }

    public List<PrefabEntry> prefabList = new List<PrefabEntry>();
    private Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

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
            }
            else
            {
                Debug.LogWarning($"PrefabManager: Duplicate prefab name found: {entry.prefabName}. Only the first one will be used.");
            }
        }
    }

    /// <summary>
    /// 지정된 이름의 프리팹을 로드하여 인스턴스화합니다.
    /// </summary>
    /// <param name="prefabName">로드할 프리팹의 이름</param>
    /// <param name="parent">새로 생성될 오브젝트의 부모 Transform (선택 사항)</param>
    /// <returns>생성된 GameObject 또는 null (프리팹을 찾지 못했을 경우)</returns>
    public GameObject InstantiatePrefab(string prefabName, Transform parent = null)
    {
        if (prefabDictionary.TryGetValue(prefabName, out GameObject prefab))
        {
            GameObject newObject = Instantiate(prefab, parent.position, quaternion.identity,null);
            newObject.name = prefabName; // 이름 설정
            return newObject;
        }
        else
        {
            Debug.LogError($"PrefabManager: Prefab with name '{prefabName}' not found.");
            return null;
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
    public GameObject LoadPrefabFromResources(string path)
    {
        GameObject loadedPrefab = Resources.Load<GameObject>(path);
        if (loadedPrefab != null)
        {
            if (!prefabDictionary.ContainsKey(loadedPrefab.name))
            {
                prefabDictionary.Add(loadedPrefab.name, loadedPrefab);
                Debug.Log($"PrefabManager: Dynamically loaded and added '{loadedPrefab.name}' to dictionary.");
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