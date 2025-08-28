using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor;
using TMPro;
using PublicDataType; // List 사용을 위해 추가
public class Guard : MonoBehaviour
{
    // NPC의 상태를 명확하게 정의합니다.
    public enum NPCState { PATROL, CHASE, ATTACK }

    [Header("상태 확인")]
    [SerializeField] private NPCState currentState = NPCState.PATROL;

    [Header("순찰 설정")]
    public List<Transform> patrolPoints; // 순찰 지점들
    public float patrolSpeed = 2f;   // 순찰 속도
    private int currentPatrolIndex = 0;

    [Header("추격 및 공격 설정")]
    public float chaseSpeed = 6f;      // 추격 속도
    public float detectionRadius = 15f;  // 적 감지 반경
    public float attackRange = 2f;     // 공격 가능 사거리
    public float attackRate = 1f;      // 초당 공격 횟수
    public LayerMask enemyLayer;       // 적 레이어
    private Transform currentTarget;
    private float nextAttackTime = 0f;

    Animator anim;

    public CharacterModelChanger cmc;

    void Start()
    {
        cmc = GetComponent<CharacterModelChanger>();
        cmc.ChangeCharacterModel(DataManager.Instance.GetJobData()[1].sprite);
        anim = GetComponentInChildren<Animator>();
        patrolSpeed += patrolSpeed * DataManager.Instance.GetJobData()[1].rank;
        anim.SetFloat("Speed", 1);
        GetComponentInChildren<TextMeshPro>().text = ((NameType)DataManager.Instance.GetJobData()[1].name).ToString();
        // 첫 번째 순찰 지점으로 이동 시작
        if (patrolPoints.Count > 0)
        {
            currentState = NPCState.PATROL;
        }
        else
        {
            Debug.LogError("순찰 지점이 설정되지 않았습니다!");
        }
    }

    void Update()
    {
        // 현재 상태에 따라 행동을 결정합니다.
        switch (currentState)
        {
            case NPCState.PATROL:
                Patrol();
                break;
            case NPCState.CHASE:
                Chase();
                break;
            case NPCState.ATTACK:
                Attack();
                break;
        }
    }

    // --- 상태별 행동 함수 ---

    private void Patrol()
    {
        // 1. 주변에 적이 있는지 확인하고, 있으면 추격 상태로 전환합니다.
        if (FindClosestEnemy())
        {
            currentState = NPCState.CHASE;
            return;
        }

        // 2. 현재 순찰 지점으로 이동합니다.
        Transform targetPoint = patrolPoints[currentPatrolIndex];
        MoveToTarget(targetPoint, patrolSpeed);

        // 3. 순찰 지점에 도착하면 다음 지점으로 목표를 변경합니다.
        if (Vector3.Distance(transform.position, targetPoint.position) < 1.5f)
        {
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Count;
        }
    }

    private void Chase()
    {
        // 1. 타겟이 사라졌는지(죽거나 비활성화) 확인합니다.
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            // 타겟이 사라졌으면 다른 적을 찾고, 없으면 순찰 상태로 돌아갑니다.
            if (!FindClosestEnemy())
            {
                currentState = NPCState.PATROL;
            }
            return;
        }

        // 2. 타겟을 향해 이동합니다.
        MoveToTarget(currentTarget, chaseSpeed);

        // 3. 타겟이 공격 범위 안에 들어오면 공격 상태로 전환합니다.
        if (Vector3.Distance(transform.position, currentTarget.position) <= attackRange)
        {
            currentState = NPCState.ATTACK;
        }
    }

    private void Attack()
    {
        // 1. 타겟이 사라졌는지 확인합니다. (Chase와 동일)
        if (currentTarget == null || !currentTarget.gameObject.activeInHierarchy)
        {
            if (!FindClosestEnemy())
            {
                currentState = NPCState.PATROL;
            } else {
                currentState = NPCState.CHASE; // 새로운 타겟을 찾았으면 다시 추격
            }
            return;
        }

        // 2. 타겟이 공격 범위를 벗어나면 다시 추격 상태로 전환합니다.
        if (Vector3.Distance(transform.position, currentTarget.position) > attackRange)
        {
            currentState = NPCState.CHASE;
            return;
        }

        // 3. 공격 쿨타임이 되었다면 공격합니다.
        FaceTarget(currentTarget); // 공격하기 전 타겟을 바라보게 함
        if (Time.time >= nextAttackTime)
        {
            // 실제 공격 로직 (애니메이션, 발사체 생성 등)
            Debug.Log(currentTarget.name + "을(를) 공격!");
            
            // 다음 공격 시간 설정
            nextAttackTime = Time.time + 1f / attackRate;
        }
    }

    // --- 보조 함수 ---

    private bool FindClosestEnemy()
    {
        Collider[] enemies = Physics.OverlapSphere(transform.position, detectionRadius, enemyLayer);
        float closestDistance = float.MaxValue;
        Transform closestEnemy = null;

        foreach (var enemy in enemies)
        {
            float distance = Vector3.Distance(transform.position, enemy.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = enemy.transform;
            }
        }

        if (closestEnemy != null)
        {
            currentTarget = closestEnemy;
            return true;
        }

        currentTarget = null;
        return false;
    }

    private void MoveToTarget(Transform target, float speed)
    {
        FaceTarget(target);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void FaceTarget(Transform target)
    {
        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // NPC가 위아래로 기울지 않도록
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    // 디버깅용: 감지 반경과 공격 범위를 씬 뷰에 표시
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}