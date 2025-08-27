using UnityEngine;

public class MoveAndDisappear : MonoBehaviour
{
    // 인스펙터 창에서 목표 위치를 지정하거나, 다른 스크립트에서 이 변수에 접근하여 설정할 수 있습니다.
    public Vector3 targetPosition;

    // 초당 이동할 속도를 정합니다.
    public float speed = 5f;

 // 포물선 경로 계산에 필요한 세 점
    private Vector3 startPoint;
    private Vector3 endPoint;
    private Vector3 controlPoint; // 랜덤하게 정해질 중간 제어점

    // 이동에 걸리는 시간 관련 변수
    private float journeyTime = 0.5f; // 아이템이 날아가는 데 걸리는 시간 (이 값을 조절해 속도 제어)
    private float startTime;

    // 인벤토리가 이 함수를 호출해 이동을 시작시킴
    public void StartMove(Transform tf)
    {
        

        // 1. 경로 설정
        this.startPoint = transform.position;
        this.endPoint = tf.position;
        
        // 2. 랜덤한 제어점 계산 (포물선의 모양 결정)
        // 시작과 끝점의 중간 지점을 기준으로 랜덤한 높이와 수평 오프셋을 추가합니다.
        Vector3 centerPoint = (startPoint + endPoint) / 2;
        float randomHeight = Random.Range(3.0f, 6.0f);   // 포물선의 최대 높이를 랜덤하게
        float randomSideOffset = Random.Range(-2.0f, 2.0f); // 좌우로 얼마나 휠지 랜덤하게
        
        this.controlPoint = centerPoint + (Vector3.up * randomHeight) + (transform.right * randomSideOffset);

        // 3. 이동 시작 시간 기록 및 상태 변경
        this.startTime = Time.time;
        

       
    }

    void Update()
    {
        

        // 현재 시간 기준으로 이동이 얼마나 진행됐는지 계산 (0에서 1 사이의 값)
        float t = (Time.time - startTime) / journeyTime;
        t = Mathf.Clamp01(t); // t가 1을 넘지 않도록 보정

        // 베지어 곡선 공식을 이용해 현재 프레임의 위치 계산
        transform.position = GetQuadraticBezierPoint(startPoint, controlPoint, endPoint, t);

        // 이동이 완료되었는지 확인
        if (t >= 0.8f)
        {
           
            // (이 부분은 이전과 동일)
            // targetInventory.FinalizeAddItem(item);
            // PrefabManager.Instance.Release(gameObject);
            
            // 임시 테스트용: 도착하면 1초 뒤 사라지게 처리
            Debug.Log("도착!");
            enabled = false;
            PrefabManager.Instance.Release(gameObject); 
        }
    }

    /// <summary>
    /// 이차 베지어 곡선 위의 한 점을 계산하는 함수
    /// </summary>
    /// <param name="p0">시작점</param>
    /// <param name="p1">제어점 (중간점)</param>
    /// <param name="p2">끝점</param>
    /// <param name="t">시간 (0~1)</param>
    /// <returns></returns>
    private Vector3 GetQuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        // 공식: (1-t)^2 * P0 + 2 * (1-t) * t * P1 + t^2 * P2
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;

        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;

        return p;
    }
}