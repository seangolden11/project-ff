using UnityEngine;
using System.Collections.Generic;

public class GrassSpawner : MonoBehaviour
{public GameObject grassPrefab;    // 풀 프리팹 (Inspector에서 할당)

    [Header("풀 생성 설정")]
    public float spawnRadius = 10f;      // 풀이 생성될 반경 (이 스포너의 중심에서)
    public int maxGrassCount = 50;      // 최대로 유지할 풀의 개수
    public float spawnInterval = 1f;    // 풀을 생성할 주기

    [Header("풀 파괴 설정")]
    // public float destroyDistanceMultiplier = 1.5f; // 스포너 콜라이더 크기의 몇 배 밖에서 풀을 파괴할지
    
    private bool _isPlayerInside = false; // 플레이어가 범위 내에 있는지 여부
    private float _spawnTimer;
    private List<GameObject> _spawnedGrasses = new List<GameObject>(); // 생성된 풀들을 관리할 리스트

    // 이 스포너에 붙은 콜라이더의 참조
    private Collider _spawnerCollider;

    void Awake()
    {
        _spawnerCollider = GetComponent<Collider>();
        if (_spawnerCollider == null)
        {
            Debug.LogError("TriggerGrassSpawner requires a Collider component on the same GameObject!");
            enabled = false; // 콜라이더가 없으면 스크립트 비활성화
            return;
        }
        _spawnerCollider.isTrigger = true; // 반드시 트리거로 설정
    }

    void Start()
    {
        grassPrefab = PrefabManager.Instance.GetPrefab("Grass");
    }

    void Update()
    {
        if (_isPlayerInside)
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= spawnInterval)
            {
                _spawnTimer = 0f;
                TrySpawnGrass();
            }
        }
    }

    // 플레이어가 트리거에 진입했을 때 호출
    void OnTriggerEnter(Collider other)
    {
        // "Player" 레이어를 가진 오브젝트인지 확인 (플레이어 레이어는 미리 설정해야 함)
        if (other.CompareTag("Player")) // 또는 other.gameObject.layer == LayerMask.NameToLayer("Player")
        {
            _isPlayerInside = true;
            Debug.Log("Player entered spawn zone. Grass spawning activated.");
        }
    }

    // 플레이어가 트리거에서 이탈했을 때 호출
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            Debug.Log("Player exited spawn zone. Grass spawning deactivated.");
            // 플레이어가 나가면 풀을 바로 파괴하거나 점진적으로 파괴할 수 있음
            // ClearAllGrasses(); 
        }
    }

    void TrySpawnGrass()
    {
        if (_spawnedGrasses.Count >= maxGrassCount)
        {
            return;
        }

        Vector3 randomPosition = GetRandomSpawnPosition();
        if (randomPosition != Vector3.zero)
        {
            GameObject newGrass = Instantiate(grassPrefab, randomPosition, Quaternion.identity, this.transform);
            _spawnedGrasses.Add(newGrass);
            // Optional: 새롭게 생성된 풀의 로컬 스케일을 (1,1,1)로 설정하여 부모 스케일 영향 최소화
            newGrass.transform.localScale = Vector3.one;
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Spawner 오브젝트를 중심으로 spawnRadius 내에서 랜덤 위치 생성
        Vector2 randomCirclePoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnOrigin = Vector3.up + new Vector3(randomCirclePoint.x, 0, randomCirclePoint.y);
        return spawnOrigin;
    }

    // 디버그용 시각화
    
}