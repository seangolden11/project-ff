using Unity.VisualScripting;
using UnityEngine;

public class Bear : Animals
{

    public Inventory inventory;
    int isNearPlayer = 0;

    float nearTimer = 0;
    public float nearLimit = 10f;

    public Item bearitem;

    public Transform trans;

    void Start()
    {
        currentState = AnimalState.Wandering; // 게임 시작 시 닭은 배회 상태로 시작
        feedTimer = 0f; // 풀 찾기 타이머 초기화
        SetNewTargetPosition(); // 초기 목표 위치 설정
        grassTag = "Animal";
        animator = GetComponentInChildren<Animator>();
        nearLimit -= DataManager.Instance.GetUpgradeData(8).level;
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
                // 정지 시간이 끝나면 다시 배회 상태로 전환
                animator.SetFloat("Speed", 0);
                if (Time.time >= nextActionTime)
                {
                    currentState = AnimalState.Wandering;
                    SetNewTargetPosition(); // 새로운 배회 목표 지점 설정
                }
                break;

            case AnimalState.SeekingGrass:
                animator.SetFloat("Speed", 1);
                // 만약 목표 풀이 없거나 비활성화되었다면, 다시 배회 상태로 돌아갑니다.
                if (currentGrassTarget == null || !currentGrassTarget.activeInHierarchy)
                {
                    Debug.Log("풀 오브젝트가 사라지거나 비활성화되었습니다. 다시 배회합니다.");
                    if (hungryTimer == 0)
                        currentState = AnimalState.Wandering;
                    else
                        currentState = AnimalState.hungry;
                    SetNewTargetPosition();
                    feedTimer = feedInterval * 0.6f; // 다음 풀 찾기 주기를 위해 타이머 초기화
                    return; // 이번 프레임의 나머지 로직 건너뛰기
                }

                // 풀 오브젝트를 향해 이동
                transform.position = Vector3.MoveTowards(transform.position, currentGrassTarget.transform.position, moveSpeed * Time.deltaTime);
                LookAtTarget(currentGrassTarget.transform.position);

                // 풀에 충분히 도달했다면 (먹이 활동)
                if (Vector3.Distance(transform.position, currentGrassTarget.transform.position) < 0.2f) // 약간 더 작은 거리로 "먹이"를 먹었다고 판단
                {
                    Debug.Log("풀에 도달하여 비활성화합니다: " + currentGrassTarget.name);
                    PrefabManager.Instance.Release(currentGrassTarget); // 풀 오브젝트 비활성화
                    currentGrassTarget = null; // 타겟 초기화

                    moveSpeed = 1f;
                    hungryTimer = 0;
                    minIdleTime = 4f;
                    maxIdleTime = 10f;

                    // 먹이 활동 후에는 다시 배회 상태로 돌아감
                    currentState = AnimalState.Wandering;
                    SetNewTargetPosition(); // 새로운 배회 목표 설정
                    feedTimer = 0f; // 타이머를 초기화하여 다음 풀 찾기까지 다시 10초를 기다립니다.
                }
                break;
            case AnimalState.hungry:
                hungryTimer += Time.deltaTime;
                moveSpeed = 3f;
                minIdleTime = 1f;
                maxIdleTime = 1f;

                if (hungryTimer >= 30f)
                {
                    trans.localScale = Vector3.zero;
                    PrefabManager.Instance.Release(gameObject);
                }
                animator.SetFloat("Speed", 1);
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
                LookAtTarget(targetPosition);

                if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
                {
                    SetNewTargetPosition();
                }


                break;


        }
        if (isNearPlayer > 0)
        {
            moveSpeed = 0.5f;
            nearTimer += Time.deltaTime;
            trans.localScale = new Vector3 (1,1,1) * (nearTimer / nearLimit);
            if (nearTimer >= nearLimit)
            {
                nearTimer = 0;
                isNearPlayer = 0;
                trans.localScale = Vector3.zero;

                PrefabManager.Instance.Get("Enemy",transform.position,transform.rotation);
                PrefabManager.Instance.Release(gameObject);
                
            }
        }
        else
        {
            moveSpeed = 1f;
        }
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Inventory"))
        {
            isNearPlayer++;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Inventory"))
        {
            isNearPlayer--;
        }
    }
}

