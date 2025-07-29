using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 필요
using System.Collections; // 코루틴을 사용하기 위해 필요

public class TimerUI : MonoBehaviour
{
    private float currentTime;    // 현재 남은 시간
    public TextMeshProUGUI timerText; // UI TextMeshPro 컴포넌트 참조
    public StageData stageData;

    private bool isTimerRunning = false; // 타이머 실행 상태

    void Start()
    {
        stageData = GameManager.Instance.stageData;
        currentTime = stageData.timeLimit;
        UpdateTimerUI(); // 초기 UI 업데이트

        // 게임 시작과 함께 타이머 시작 (원하는 시점에 StartTimer() 호출 가능)
        StartTimer();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            // 시간이 0보다 크면 계속 감소
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime; // 프레임당 시간 감소
                UpdateTimerUI(); // UI 업데이트
            }
            else // 시간이 0이 되면 타이머 중지 및 게임 오버 처리
            {
                currentTime = 0; // 음수 방지
                UpdateTimerUI(); // 최종 UI 업데이트
                StopTimer(); // 타이머 중지
                Debug.Log("시간 종료!");
                // 여기에 게임 오버, 재시작 등 추가 로직 구현
            }
        }
    }

    // 타이머 UI를 업데이트하는 함수
    void UpdateTimerUI()
    {
        // 남은 시간을 분:초 형식으로 포맷
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);

        // 두 자리 숫자로 포맷팅 (예: 05:07)
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    // 타이머 시작 함수
    public void StartTimer()
    {
        isTimerRunning = true;
    }

    // 타이머 중지 함수
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    // 타이머 재설정 함수
    public void ResetTimer()
    {
        currentTime = stageData.timeLimit;
        UpdateTimerUI();
        isTimerRunning = false; // 재설정 후에는 자동으로 시작하지 않음
    }
}