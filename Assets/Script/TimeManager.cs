using UnityEngine;
using System; // Action 이벤트를 사용하기 위해 필요

public class TimerManager : MonoBehaviour
{
    public float currentTime { get; private set; } // 현재 남은 시간
    public float spawnTime { get; private set; } // 스폰 시간

    public event Action<float> OnTimerTick; // 시간 업데이트를 알리는 이벤트
    public event Action OnTimerEnd; // 타이머가 0에 도달했을 때 알리는 이벤트

    private float _initialTimeLimit; // 초기 시간 제한

    public int starCount =0;

    /// <summary>
    /// 주어진 시간 제한으로 타이머를 초기화합니다.
    /// </summary>
    /// <param name="timeLimit">타이머의 시작 시간(초 단위).</param>
    void Start()
    {
        _initialTimeLimit = GameManager.Instance.stageData.timeLimit;
        currentTime = _initialTimeLimit;
        GameManager.Instance.OnStageStart();
        if (Time.timeScale == 0)
        {
            StartTimer();
        }
        
    }

    void Update()
    {

        if (currentTime > 0)
        {
            currentTime -= Time.deltaTime; // 프레임당 시간 감소
            OnTimerTick?.Invoke(currentTime); // 리스너에게 시간 변경 알림
            spawnTime += Time.deltaTime;
            if (spawnTime >= 50)
            {
                SpawnBear();
                spawnTime = 0;
            }
        }
        else if (starCount < 3)
        {
            starCount++;
            currentTime = 120;
            OnTimerTick?.Invoke(currentTime); // 리스너에게 시간 변경 알림
        }
        else
        {
            currentTime = 0; // 음수 방지
            StopTimer(); // 타이머 중지
            OnTimerTick?.Invoke(currentTime); // 최종 UI 업데이트
            OnTimerEnd?.Invoke(); // 리스너에게 타이머가 종료되었음을 알림
            Debug.Log("시간 종료!");
        }
        
        
    }

    public void SpawnBear()
    {
        PrefabManager.Instance.Get("Bear", transform.position, transform.rotation);
    }

    /// <summary>
    /// 타이머를 시작합니다.
    /// </summary>
    public void StartTimer()
    {
        Time.timeScale = 1;
    }

    /// <summary>
    /// 타이머를 중지합니다.
    /// </summary>
    public void StopTimer()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// 타이머를 초기 시간 제한으로 재설정합니다.
    /// </summary>
    public void ResetTimer()
    {
        currentTime = _initialTimeLimit;
        OnTimerTick?.Invoke(currentTime); // 재설정 후 UI 업데이트
    }
}