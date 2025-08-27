using UnityEngine;
using System.Collections.Generic;

public class GrassSpawner : MonoBehaviour
{

    [Header("풀 생성 설정")]
    public float spawnRadius = 10f;      // 풀이 생성될 반경 (이 스포너의 중심에서)
    public int maxGrassCount = 50;      // 최대로 유지할 풀의 개수
    public float spawnInterval = 2f;    // 풀을 생성할 주기
    
    private bool _isPlayerInside = false; // 플레이어가 범위 내에 있는지 여부
    private float _spawnTimer;

    public int level = 1;

    Animator anim;

    void Update()
    {
        if (_isPlayerInside)
        {
            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= spawnInterval - level * 0.3f)
            {
                _spawnTimer = 0f;
                TrySpawnGrass();
            }
        }
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    // 플레이어가 트리거에 진입했을 때 호출
    void OnTriggerEnter(Collider other)
    {
        // "Player" 레이어를 가진 오브젝트인지 확인 (플레이어 레이어는 미리 설정해야 함)
        if (other.CompareTag("Player")) // 또는 other.gameObject.layer == LayerMask.NameToLayer("Player")
        {
            _isPlayerInside = true;
            anim.SetBool("isWorking", true);
            Debug.Log("Player entered spawn zone. Grass spawning activated.");
        }
    }

    // 플레이어가 트리거에서 이탈했을 때 호출
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isPlayerInside = false;
            anim.SetBool("isWorking", false);
            Debug.Log("Player exited spawn zone. Grass spawning deactivated.");
            // 플레이어가 나가면 풀을 바로 파괴하거나 점진적으로 파괴할 수 있음
            // ClearAllGrasses(); 
        }
    }

    void TrySpawnGrass()
    {
        if (MoneyManager.Instance.TryRemoveMoney(6 - level))
        {
            Vector3 randomPosition = GetRandomSpawnPosition();
            if (randomPosition != Vector3.zero)
            {
                GameObject newGrass = PrefabManager.Instance.Get("Grass", randomPosition, Quaternion.identity);
            }
        }
    }

    Vector3 GetRandomSpawnPosition()
    {
        // spawnRadius 내에서 랜덤 위치 생성
        Vector2 randomCirclePoint = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnOrigin = new Vector3(randomCirclePoint.x, 0.5f, randomCirclePoint.y);
        return spawnOrigin;
    }

    
}