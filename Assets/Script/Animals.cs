using UnityEngine;
using System.Linq; // LINQ를 사용하지 않아도 되지만, 혹시 모를 확장을 위해 추가

public class Animals : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 1f;
    public Vector2 minBounds = new Vector2(-10f, -10f); // X, Z 최소 범위
    public Vector2 maxBounds = new Vector2(10f, 10f);    // X, Z 최대 범위
    public float minMoveTime = 1f; // 최소 이동 시간
    public float maxMoveTime = 3f; // 최대 이동 시간

    public float minIdleTime = 4f; // 최소 정지 시간
    public float maxIdleTime = 10f; // 최대 정지 시간

    [Header("Feeding Behavior")]
    public string grassTag = "Grass"; // 풀 오브젝트를 식별할 태그 (Unity에서 풀 오브젝트에 이 태그를 지정해주세요!)
    public float feedInterval = 10f; // 닭이 풀을 찾을 간격 (초)
    public float grassSearchRadius = 15f; // 닭이 풀을 찾을 반경

    private Vector3 targetPosition;
    private float nextActionTime; // 다음 움직이거나 정지할 시간
    private float feedTimer; // 풀을 찾기 위한 타이머

    private GameObject currentGrassTarget; // 현재 목표로 하는 풀 오브젝트

    // 닭의 상태를 관리하는 Enum
    private enum AnimalState { Wandering, Idling, SeekingGrass }
    private AnimalState currentState;

    void Start()
    {
        currentState = AnimalState.Wandering; // 게임 시작 시 닭은 배회 상태로 시작
        feedTimer = 0f; // 풀 찾기 타이머 초기화
        SetNewTargetPosition(); // 초기 목표 위치 설정
    }

    void Update()
    {
        // 풀 찾기 타이머를 증가시킵니다.
        feedTimer += Time.deltaTime;

        // 1. 일정 시간이 되면 풀을 찾아 이동할 준비를 합니다.
        // 현재 풀을 찾아 이동 중이 아닐 때만 풀을 찾습니다.
        if (feedTimer >= feedInterval && currentState != AnimalState.SeekingGrass)
        {
            FindAndTargetGrass();
        }

        // 2. 현재 닭의 상태에 따라 다른 행동을 수행합니다.
        switch (currentState)
        {
            case AnimalState.Wandering:
                // 목표 위치로 이동
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

                // 목표 위치에 거의 도달했거나, 랜덤 이동 시간이 끝나면 정지 상태로 전환
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f || Time.time >= nextActionTime)
                {
                    currentState = AnimalState.Idling;
                    nextActionTime = Time.time + Random.Range(minIdleTime, maxIdleTime); // 다음 정지 시간 설정
                }
                break;

            case AnimalState.Idling:
                // 정지 시간이 끝나면 다시 배회 상태로 전환
                if (Time.time >= nextActionTime)
                {
                    currentState = AnimalState.Wandering;
                    SetNewTargetPosition(); // 새로운 배회 목표 지점 설정
                }
                break;

            case AnimalState.SeekingGrass:
                // 만약 목표 풀이 없거나 비활성화되었다면, 다시 배회 상태로 돌아갑니다.
                if (currentGrassTarget == null || !currentGrassTarget.activeInHierarchy)
                {
                    Debug.Log("풀 오브젝트가 사라지거나 비활성화되었습니다. 다시 배회합니다.");
                    currentState = AnimalState.Wandering;
                    SetNewTargetPosition();
                    feedTimer = feedInterval; // 다음 풀 찾기 주기를 위해 타이머 초기화
                    return; // 이번 프레임의 나머지 로직 건너뛰기
                }

                // 풀 오브젝트를 향해 이동
                transform.position = Vector3.MoveTowards(transform.position, currentGrassTarget.transform.position, moveSpeed * Time.deltaTime);

                // 풀에 충분히 도달했다면 (먹이 활동)
                if (Vector3.Distance(transform.position, currentGrassTarget.transform.position) < 0.2f) // 약간 더 작은 거리로 "먹이"를 먹었다고 판단
                {
                    Debug.Log("풀에 도달하여 비활성화합니다: " + currentGrassTarget.name);
                    currentGrassTarget.SetActive(false); // 풀 오브젝트 비활성화
                    this.GetComponent<AnimalGive>().SpawnGoods();
                    currentGrassTarget = null; // 타겟 초기화

                    // 먹이 활동 후에는 다시 배회 상태로 돌아감
                    currentState = AnimalState.Wandering;
                    SetNewTargetPosition(); // 새로운 배회 목표 설정
                    feedTimer = 0f; // 타이머를 초기화하여 다음 풀 찾기까지 다시 10초를 기다립니다.
                }
                break;
        }
    }

    // 새로운 랜덤 목표 위치를 설정합니다 (배회 상태에서 사용)
    void SetNewTargetPosition()
    {
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomZ = Random.Range(minBounds.y, maxBounds.y);

        targetPosition = new Vector3(randomX, transform.position.y, randomZ); // Y 값은 현재 높이 유지

        nextActionTime = Time.time + Random.Range(minMoveTime, maxMoveTime); // 다음 이동 시간 설정
    }

    // 주변에서 활성화된 풀 오브젝트를 찾아 목표로 설정합니다.
    void FindAndTargetGrass()
    {
        // 씬 내의 모든 "Grass" 태그를 가진 오브젝트를 찾습니다.
        GameObject[] allGrass = GameObject.FindGameObjectsWithTag(grassTag);
        
        if (allGrass.Length > 0)
        {
            GameObject nearestActiveGrass = null;
            float minDistance = Mathf.Infinity;

            // 활성화된 풀 오브젝트 중에서 가장 가까운 것을 찾습니다.
            foreach (GameObject grass in allGrass)
            {
                if (grass.activeInHierarchy) // 현재 활성화되어 있는 풀만 고려
                {
                    float distance = Vector3.Distance(transform.position, grass.transform.position);
                    if (distance < minDistance && distance <= grassSearchRadius) // 탐색 반경 내에 있는 풀만 고려
                    {
                        minDistance = distance;
                        nearestActiveGrass = grass;
                    }
                }
            }

            if (nearestActiveGrass != null)
            {
                currentGrassTarget = nearestActiveGrass;
                currentState = AnimalState.SeekingGrass; // 풀을 찾아 이동하는 상태로 변경
                Debug.Log("풀을 목표로 설정했습니다: " + currentGrassTarget.name);
            }
            else
            {
                // 주변에 먹을 수 있는 활성화된 풀이 없다면 계속 배회 상태 유지
                Debug.Log("탐색 반경 내에 활성화된 풀이 없습니다. 배회 상태를 유지합니다.");
                feedTimer = 0f; // 풀을 찾지 못했으므로 타이머를 초기화하여 다음 주기에 다시 시도
            }
        }
        else
        {
            // 씬에 "Grass" 태그를 가진 오브젝트가 전혀 없다면
            Debug.Log("씬에 '" + grassTag + "' 태그를 가진 풀 오브젝트가 없습니다.");
            feedTimer = 0f; // 타이머를 초기화하여 다음 주기에 다시 시도
        }
    }

    // 유니티 에디터에서 디버깅을 위한 기즈모 그리기
    void OnDrawGizmosSelected()
    {
        // 배회 범위 표시
        Gizmos.color = Color.green;
        Vector3 center = new Vector3((minBounds.x + maxBounds.x) / 2, transform.position.y, (minBounds.y + maxBounds.y) / 2);
        Vector3 size = new Vector3(maxBounds.x - minBounds.x, 0.1f, maxBounds.y - minBounds.y);
        Gizmos.DrawWireCube(center, size);

        // 현재 목표 위치 표시 (배회 중일 때 파란색, 풀을 찾을 때 빨간색)
        if (currentState == AnimalState.Wandering)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(targetPosition, 0.5f);
            Gizmos.DrawLine(transform.position, targetPosition);
        }
        else if (currentState == AnimalState.SeekingGrass && currentGrassTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(currentGrassTarget.transform.position, 0.5f);
            Gizmos.DrawLine(transform.position, currentGrassTarget.transform.position);
        }

        // 풀 탐색 반경 표시
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, grassSearchRadius);
    }
}