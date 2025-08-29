using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting; // List 사용을 위해 추가
public class Worker : MonoBehaviour
{
    // NPC 상태 정의
    public enum NPCState { IDLE, MOVING_TO_PRIMARY, PICKING_UP, MOVING_TO_SECONDARY, DROPPING_OFF }

    [Header("NPC 설정")]
    float moveSpeed = 6.0f;
    float stopDistance = 1.5f;

    [Header("상태 확인")]
    public NPCState currentState;

    public Transform primaryBuildingTarget;
    public Building secondaryBuildingTarget;
    public Item carriedItem;
    public Item itemType;

    public int itemCount;

    Animator anim;

    public Transform startTrans;

   

    public int rank;

    // 최적화를 위해 건물 목록을 캐싱
    // 실제 게임에서는 BuildingManager 같은 클래스가 이 목록을 관리하는 것이 좋습니다.


    void Start()
    {
        itemCount = 0;

        currentState = NPCState.MOVING_TO_PRIMARY;
        ResetToIdle();
        anim = GetComponentInChildren<Animator>();
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickableItem"))
        {
            if (itemCount < rank && itemType == other.GetComponent<ItemPickup>().item)
            {
                itemCount++;
                PrefabManager.Instance.Release(other.gameObject);
            }
            if (itemCount == rank)
            {
                currentState = NPCState.MOVING_TO_SECONDARY;
            }
        }
    }

    void Update()
    {
        // 상태에 따라 행동 결정
        switch (currentState)
        {
            case NPCState.IDLE:
                anim.SetFloat("Speed", 0);
                ResetToIdle();
                break;

            case NPCState.MOVING_TO_PRIMARY:
                anim.SetFloat("Speed", 1);
                MoveToTarget(primaryBuildingTarget);
                if (Vector3.Distance(transform.position, primaryBuildingTarget.position) < stopDistance)
                {
                    currentState = NPCState.PICKING_UP;
                }
                break;

            case NPCState.PICKING_UP:
                PickUpItem();
                break;

            case NPCState.MOVING_TO_SECONDARY:
                // 이동 중 예외 상황 처리
                if (HandleTransportExceptions())
                {
                    // 예외가 발생했다면(true 반환), 현재 프레임의 나머지 로직은 중단
                    return;
                }
                anim.SetFloat("Speed", 1);
                MoveToTarget(secondaryBuildingTarget.transform);
                if (Vector3.Distance(transform.position, secondaryBuildingTarget.transform.position) < stopDistance)
                {
                    currentState = NPCState.DROPPING_OFF;
                }
                break;

            case NPCState.DROPPING_OFF:
                DropOffItem();
                break;
        }
    }

    // --- 상태별 행동 함수 ---

    private void FindWork()
    {
        // 생산이 끝난 1차 건물을 찾습니다.
        Transform availablePrimary = FindAvailablePrimaryBuilding();
        if (availablePrimary != null)
        {
            primaryBuildingTarget = availablePrimary;

            // 아이템을 운반할 2차 건물을 찾습니다.
            Building availableSecondary = FindClosestAvailableSecondaryBuilding(transform.position);
            if (availableSecondary != null)
            {
                secondaryBuildingTarget = availableSecondary;
                currentState = NPCState.MOVING_TO_PRIMARY;
                // Debug.Log(primaryBuildingTarget.name + "에서 아이템을 가져와 " + secondaryBuildingTarget.name + "으로 운반 시작!");
            }
            else
            {
                // 당장 일할 수 있는 2차 건물이 없으면 대기
                
            }
        }
        
        

    }

    private void PickUpItem()
    {
        if (itemCount == rank)
        {
            currentState = NPCState.MOVING_TO_SECONDARY;
        }
        else
        {
            // 다른 NPC가 아이템을 먼저 가져간 경우
            
            anim.SetFloat("Speed",1);
            MoveToTarget(startTrans);
            if (Vector3.Distance(transform.position, startTrans.position) < stopDistance)
            {
                ResetToIdle();
            }

            
        }
    }

    private void DropOffItem()
    {
        if (itemCount == rank)
        {
            secondaryBuildingTarget.GetItem(this);
        }
        // 작업 완료 후 대기 상태로 전환
        ResetToIdle();
    }

    // --- 이동 및 예외 처리 함수 ---

    private void MoveToTarget(Transform target)
    {
        if (target == null) return;

        // 목표 방향으로 회전
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // y축으로는 회전하지 않도록
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // 앞으로 이동
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private void MoveToTargetPos(Vector3 target)
    {
        if (target == null) return;

        // 목표 방향으로 회전
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0; // y축으로는 회전하지 않도록
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }

        // 앞으로 이동
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    

    private bool HandleTransportExceptions()
    {
        // 1. 운반 중 아이템이 사라졌는지 확인
        // if (carriedItem == null)
        // {
        //     // Debug.Log("운반 중 아이템이 사라져 작업을 취소합니다.");
        //     // ResetToIdle();
        //     // return true; // 예외 발생
        // }

        // 2. 목표 건물이 갑자기 일하는 중으로 바뀌었는지 확인
        if (secondaryBuildingTarget.isWorking)
        {
            Debug.Log(secondaryBuildingTarget.name + "가 작업을 시작하여 다른 건물을 찾습니다.");
            Building newTarget = FindClosestAvailableSecondaryBuilding(transform.position, secondaryBuildingTarget);

            if (newTarget != null)
            {
                // 새로운 목표 설정
                secondaryBuildingTarget = newTarget;
                Debug.Log("새로운 목표: " + newTarget.name);
            }
            else
            {
                // 대안이 없으면 작업 포기
                Debug.Log("다른 운반 가능한 건물이 없어 작업을 취소합니다.");
                ResetToIdle();
            }
            return true; // 예외 발생 (경로 재설정 포함)
        }

        return false; // 예외 없음
    }

    // --- 탐색 헬퍼 함수 ---

    private Transform FindAvailablePrimaryBuilding()
    {

        foreach (var building in PlayerSpawner.allBuildings)
        {
            // 1차 건물이고, 아이템을 가지고 있는 건물을 찾음
            if (building.isPrimary && building.gameObject.GetComponentInChildren<ItemPickup>() != null)
            {
                if (building.itemToGive == itemType)
                    return building.gameObject.GetComponentInChildren<ItemPickup>().transform;
            }
        }
        return null;
    }

    private Building FindClosestAvailableSecondaryBuilding(Vector3 fromPosition, Building excludeBuilding = null)
    {
        Building closest = null;
        float minDistance = float.MaxValue;

        foreach (var building in PlayerSpawner.allBuildings)
        {
            // 2차 건물이고, 일하고 있지 않으며, 제외 대상이 아닌 경우
            if (!building.isPrimary && !building.isWorking && building != excludeBuilding && building.itemToTake == itemType)
            {
                float distance = Vector3.Distance(fromPosition, building.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closest = building;
                }
            }
        }
        return closest;
    }

    private void ResetToIdle()
    {
        // primaryBuildingTarget = null;
        // secondaryBuildingTarget = null;
        if (currentState != NPCState.IDLE) // 중복 실행 방지
        {
            currentState = NPCState.IDLE;
            StartCoroutine(FindWorkRoutine()); // IDLE 상태가 되면 코루틴 시작
        }
    }
    
    private System.Collections.IEnumerator FindWorkRoutine()
{
    // IDLE 상태인 동안 계속 반복
    while (currentState == NPCState.IDLE)
    {
        FindWork(); // 실제 탐색 함수 호출

        // 0.5초 동안 대기한 후 다시 시도 (이 시간을 조절하여 성능과 반응성 사이의 균형을 맞춥니다)
        yield return new WaitForSeconds(0.5f); 
    }
}
}