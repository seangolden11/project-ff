using UnityEngine;
using TMPro;

public class TimerUI : MonoBehaviour
{
    public TextMeshProUGUI timerText; // UI TextMeshPro 컴포넌트 참조
    public StageData stageData; // StageData가 timeLimit을 가지고 있다고 가정
    public TimerManager timerManager; // 새로운 TimerManager 참조

    void Start()
    {
        // 참조 가져오기
        if (GameManager.Instance != null)
        {
            stageData = GameManager.Instance.stageData;
        }
        else
        {
            Debug.LogError("GameManager.Instance가 null입니다. GameManager가 초기화되었는지 확인하세요.");
            return;
        }

        // 씬에서 TimerManager를 찾거나 없으면 생성합니다.
        // 싱글톤 패턴을 사용할 경우 GameManager에서 관리하는 것이 더 좋습니다.
        timerManager = FindFirstObjectByType<TimerManager>();
        if (timerManager == null)
        {
            GameObject timerManagerGO = new GameObject("TimerManager");
            timerManager = timerManagerGO.AddComponent<TimerManager>();
        }
        timerManager.OnTimerTick += UpdateTimerUI; // 시간이 업데이트될 때마다 UI 업데이트 함수 호출
        timerManager.OnTimerEnd += HandleTimerEnd; // 타이머가 종료될 때 호출될 함수
    }

    void OnDestroy()
    {
        // 이 오브젝트가 파괴될 때 메모리 누수를 방지하기 위해 이벤트 구독을 해제합니다.
        if (timerManager != null)
        {
            timerManager.OnTimerTick -= UpdateTimerUI;
            timerManager.OnTimerEnd -= HandleTimerEnd;
        }
    }

    // TimerManager로부터 시간을 받아 UI를 업데이트하는 함수
    void UpdateTimerUI(float currentTime)
    {
        // 남은 시간을 분:초 형식으로 포맷
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // 두 자리 숫자로 포맷팅 (예: 05:07)
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // 타이머가 종료되었을 때 호출될 함수
    void HandleTimerEnd()
    {
        Debug.Log("TimerUI: 타이머가 종료되었습니다. 게임 오버 처리!");
        // 여기에 게임 오버 화면 표시, 재시작 버튼 활성화 등 UI 관련 로직을 추가합니다.
    }

}