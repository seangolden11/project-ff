using UnityEngine;

public class FollowC : MonoBehaviour
{
    [Header("카메라 팔로우 설정")]
    [SerializeField] private Transform target; // 카메라가 따라갈 대상 (플레이어)
    [SerializeField] private Vector3 offset = new Vector3(0f, 10f, -10f); // 타겟 기준 오프셋 (위치)
    

    void FixedUpdate()
    {
        if (target == null)
        {
            Debug.LogWarning("카메라 타겟이 설정되지 않았습니다!");
            return;
        }

        // 1. 목표 위치 계산: 타겟의 위치에 오프셋을 더합니다.
        // 카메라의 회전은 고려하지 않고, 순수하게 플레이어의 위치만 따라갑니다.
        Vector3 desiredPosition = target.position + offset;

        // 2. 현재 카메라 위치에서 목표 위치로 부드럽게 이동합니다.
        transform.position = Vector3.Lerp(transform.position, desiredPosition, 0.1f);

        // 3. (중요!) 카메라 회전은 고정합니다.
        // transform.LookAt(target); 와 같은 코드가 있다면 주석 처리하거나 제거해야 합니다.
        // 카메라의 초기 회전 (X축 약 80~90도)은 직접 인스펙터에서 설정되어 있어야 합니다.
    }
}