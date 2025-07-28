using UnityEngine;

public class Animals : MonoBehaviour
{
    public float moveSpeed = 1f;
    public Vector2 minBounds = new Vector2(-10f, -10f); // X, Z 최소 범위
    public Vector2 maxBounds = new Vector2(10f, 10f);    // X, Z 최대 범위
    public float minMoveTime = 1f; // 최소 이동 시간
    public float maxMoveTime = 3f; // 최대 이동 시간

    public float minIdleTime = 4f; // 최소 정지 시간 (새로 추가)
    public float maxIdleTime = 10f; // 최대 정지 시간 (새로 추가)

    private Vector3 targetPosition;
    private float nextActionTime; // 다음 움직이거나 정지할 시간
    private bool isMoving = true; // 현재 이동 중인지 정지 중인지 상태

    void Start()
    {
        SetNewTargetPosition(); // 처음에는 이동 상태로 시작
    }

    void Update()
    {
        if (isMoving)
        {
            // 목표 위치로 이동
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 목표 위치에 거의 도달했거나, 다음 이동 시간이 되면 정지 상태로 전환
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                // 정지 상태로 전환
                isMoving = false;
                nextActionTime = Time.time + Random.Range(minIdleTime, maxIdleTime); // 다음 행동은 정지 후 이동
            }
        }
        else // 정지 상태일 때
        {
            // 정지 시간이 되면 다시 이동 상태로 전환
            if (Time.time >= nextActionTime)
            {
                isMoving = true;
                SetNewTargetPosition(); // 새로운 목표 지점 설정
            }
        }
    }

    void SetNewTargetPosition()
    {
        // 범위 내에서 랜덤한 X, Z 좌표 생성
        float randomX = Random.Range(minBounds.x, maxBounds.x);
        float randomZ = Random.Range(minBounds.y, maxBounds.y);
        
        // Y 값은 현재 높이를 유지하거나, 지형에 맞춰 설정해야 함
        targetPosition = new Vector3(randomX, transform.position.y, randomZ);

        // 다음 이동 시간 설정 (이동을 시작할 때 설정)
        nextActionTime = Time.time + Random.Range(minMoveTime, maxMoveTime);
    }
}