using Unity.VisualScripting;
using UnityEngine;

public class Bear : MonoBehaviour
{
    private enum AnimalState { Wandering, Idling }

    private AnimalState currentState = AnimalState.Wandering;

    public float moveSpeed = 1f;
    public Vector2 minBounds = new Vector2(-10f, -10f); // X, Z 최소 범위
    public Vector2 maxBounds = new Vector2(10f, 10f);    // X, Z 최대 범위
    public float minMoveTime = 1f; // 최소 이동 시간
    public float maxMoveTime = 3f; // 최대 이동 시간

    public float minIdleTime = 4f; // 최소 정지 시간
    public float maxIdleTime = 10f; // 최대 정지 시간

    private Vector3 targetPosition;
    private float nextActionTime; // 다음 움직이거나 정지할 시간

    bool isNearPlayer;
    float nearTimer = 0;
    float nearLimit = 5f;

    public Animator animator;

    void Start()
    {
        currentState = AnimalState.Wandering; // 게임 시작 시 닭은 배회 상태로 시작
        nextActionTime = 0f; // 풀 찾기 타이머 초기화
        SetNewTargetPosition(); // 초기 목표 위치 설정
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        switch (currentState)
        {
            case AnimalState.Wandering:
            animator.SetFloat("Speed", 1);
                // 목표 위치로 이동
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                LookAtTarget(targetPosition);

                // 목표 위치에 거의 도달했거나, 랜덤 이동 시간이 끝나면 정지 상태로 전환
                if (Vector3.Distance(transform.position, targetPosition) < 0.1f || Time.time >= nextActionTime)
                {
                    currentState = AnimalState.Idling;
                    nextActionTime = Time.time + Random.Range(minIdleTime, maxIdleTime); // 다음 정지 시간 설정
                }
                break;

            case AnimalState.Idling:
                animator.SetFloat("Speed", 0);
                // 정지 시간이 끝나면 다시 배회 상태로 전환
                if (Time.time >= nextActionTime)
                {
                    currentState = AnimalState.Wandering;
                    SetNewTargetPosition(); // 새로운 배회 목표 지점 설정
                }
                break;
        }

        if (isNearPlayer)
        {
            moveSpeed = 0.5f;
            nearTimer += Time.deltaTime;
            if (nearTimer >= nearLimit)
            {
                PrefabManager.Instance.Release(gameObject);
            }
        }
        else
        {
            moveSpeed = 1f;
        }
    }

    void LookAtTarget(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        // Y축 회전만 필요하므로 Y값을 0으로 설정하여 수평 방향만 바라보게 합니다.
        direction.y = 0; 

        if (direction != Vector3.zero) // 방향 벡터가 0이 아닐 때만 회전
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            // 부드러운 회전을 위해 Quaternion.Slerp 사용
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f); // 5f는 회전 속도
        }
    }

    void SetNewTargetPosition()
    {
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomZ = Random.Range(minBounds.y, maxBounds.y);

        targetPosition = new Vector3(randomX, transform.position.y, randomZ); // Y 값은 현재 높이 유지

        nextActionTime = Time.time + Random.Range(minMoveTime, maxMoveTime); // 다음 이동 시간 설정
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Animal"))
        {
            PrefabManager.Instance.Release(other.gameObject);
        }
        else if (other.CompareTag("Inventory"))
        {
            isNearPlayer = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Inventory"))
        {
            isNearPlayer = false;
        }
    }
}
