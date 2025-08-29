using UnityEngine;

public class MoveAndDisappear : MonoBehaviour
{
    // --- 기존 변수들 ---
    private Vector3 startPoint;
    private Transform endPoint;
    private Vector3 controlPoint;
    private float journeyTime = 0.6f; // 이동 시간을 살짝 늘려 효과를 더 잘보이게 조정
    private float startTime;

    Vector3 origon = new Vector3(0.5f, 0.5f, 0.5f);
    private Vector3 initialScale; // 초기 크기를 저장할 변수

    // --- ✨ 새로 추가된 변수들 (인스펙터에서 조절 가능) ---

    [Tooltip("이동 속도 변화를 제어하는 커브. (예: Ease Out 커브 추천)")]
    public AnimationCurve moveCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Tooltip("초당 회전 속도")]
    public float rotationSpeed = 360f;

    [Tooltip("목표 지점 도착 시 작아지는 효과를 사용할지 여부")]
    public bool shrinkOnApproach = true;

    [Tooltip("작아지기 시작하는 시점 (0.0 ~ 1.0)")]
    [Range(0f, 1f)]
    public float shrinkStartTime = 0.5f;

    /// <summary>
    /// 인벤토리 등에서 이 함수를 호출하여 이동을 시작시킵니다.
    /// </summary>
    public void StartMove(Transform targetTransform)
    {
        // 1. 경로 설정
        this.startPoint = transform.position;
        this.endPoint = targetTransform;

        // 2. 랜덤한 제어점 계산 (더 역동적인 포물선을 위해 높이와 범위를 조금 더 늘림)
        Vector3 centerPoint = (startPoint + endPoint.position) / 2;
        float randomHeight = Random.Range(4.0f, 7.0f);
        float randomSideOffset = Random.Range(-3.0f, 3.0f);
        this.controlPoint = centerPoint + (Vector3.up * randomHeight) + (transform.right * randomSideOffset);

        // 3. 이동 시작 시간 기록 및 초기 상태 저장
        this.startTime = Time.time;
        this.initialScale = transform.localScale; // ✨ 이동 시작 시의 크기 저장

        // 오브젝트 풀에서 재사용될 경우를 대비해 활성화
        enabled = true; 
    }

    void Update()
    {
        // 1. 시간 경과율 계산 (0에서 1 사이의 값)
        float timeRatio = (Time.time - startTime) / journeyTime;

        // 2. ✨ 애니메이션 커브를 이용해 실제 이동률 계산
        // timeRatio가 선형적으로 증가할 때, curveT는 커브 모양에 따라 비선형적으로 증가합니다.
        float curveT = moveCurve.Evaluate(timeRatio);

        // 3. 베지어 곡선 공식을 이용해 현재 위치 계산
        transform.position = GetQuadraticBezierPoint(startPoint, controlPoint, endPoint.position, curveT);

        // 4. ✨ 회전 효과 추가
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);

        // 5. ✨ 목표에 가까워지면 크기 줄이는 효과
        if (shrinkOnApproach && timeRatio > shrinkStartTime)
        {
            // shrinkStartTime 시점부터 끝까지(1.0)를 기준으로 다시 0~1 사이의 값을 계산
            float shrinkRatio = Mathf.InverseLerp(shrinkStartTime, 1.0f, timeRatio);
            transform.localScale = Vector3.Lerp(initialScale, Vector3.zero, shrinkRatio);
        }

        // 6. 이동 완료 확인
        if (timeRatio >= 1.0f)
        {
            
            enabled = false; // Update 중지
            transform.localScale = origon;
            // 실제 프로젝트에서는 아래 코드를 사용하게 됩니다.
            // targetInventory.FinalizeAddItem(item);
            PrefabManager.Instance.Release(gameObject);
        }
    }

    /// <summary>
    /// 이차 베지어 곡선 위의 한 점을 계산하는 함수 (기존과 동일)
    /// </summary>
    private Vector3 GetQuadraticBezierPoint(Vector3 p0, Vector3 p1, Vector3 p2, float t)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }
}