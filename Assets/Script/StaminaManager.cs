using System;
using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 필요

public class StaminaManager : MonoBehaviour
{

    // --- 설정 변수 (인스펙터에서 조절) ---
    [Header("설정")]
    public int maxStamina = 5; // 최대 스테미너
    public float recoveryDuration = 300; // 스테미너 1개 회복에 걸리는 시간 (초). 300초 = 5분

    // --- UI 연결 변수 ---
    [Header("UI 요소")]
    public TextMeshProUGUI staminaText; // 현재 스테미너를 표시할 텍스트
    public TextMeshProUGUI timerText; // 다음 회복까지 남은 시간을 표시할 텍스트

    // --- 내부 변수 ---
    private int currentStamina;
    private DateTime lastStaminaUpdateTime; // 마지막으로 스테미너 정보가 갱신된 시간

    void Start()
    {
        LoadStamina();
        RecoverStaminaOffline();
    }

    void Update()
    {
        UpdateUI();
        CheckStaminaRecovery();
    }

    // 게임 시작 시 저장된 데이터 불러오기
    void LoadStamina()
    {
        currentStamina = DataManager.Instance.GetHeartData();
        if (currentStamina > 5)
            currentStamina = 5;
        
        string lastUpdateTimeString = DataManager.Instance.GetrecoverData();
        if (!string.IsNullOrEmpty(lastUpdateTimeString))
        {
            lastStaminaUpdateTime = DateTime.Parse(lastUpdateTimeString);
        }
        else
        {
            lastStaminaUpdateTime = DateTime.Now; // 첫 실행 시 현재 시간으로 설정
        }
    }

    // 데이터 저장하기
    void SaveStamina()
    {
        DataManager.Instance.SetrecoverData(lastStaminaUpdateTime.ToString(), currentStamina);
        DataManager.Instance.SaveGameProgress();
    }

    // 게임을 끈 동안의 스테미너 회복 계산
    void RecoverStaminaOffline()
    {
        if (currentStamina >= maxStamina) return;

        TimeSpan elapsedTime = DateTime.Now - lastStaminaUpdateTime;
        int staminaToRecover = (int)(elapsedTime.TotalSeconds / recoveryDuration);

        if (staminaToRecover > 0)
        {
            currentStamina = Mathf.Min(currentStamina + staminaToRecover, maxStamina);
            // 회복된 만큼의 시간을 마지막 업데이트 시간에 더해줌 (정확한 타이머 계산을 위함)
            lastStaminaUpdateTime = lastStaminaUpdateTime.AddSeconds(staminaToRecover * recoveryDuration);
            SaveStamina();
        }
    }

    // 실시간으로 스테미너가 회복되는지 체크
    void CheckStaminaRecovery()
    {
        if (currentStamina >= maxStamina) return;

        TimeSpan elapsedTime = DateTime.Now - lastStaminaUpdateTime;
        if (elapsedTime.TotalSeconds >= recoveryDuration)
        {
            currentStamina++;
            lastStaminaUpdateTime = DateTime.Now;
            SaveStamina();
        }
    }

    // 스테미너 사용 함수 (스테이지 입장 버튼 등에서 호출)
    public bool UseStamina()
    {
        if (currentStamina > 0)
        {
            // 최대 스테미너였다가 방금 하나를 사용했다면, 회복 타이머를 지금부터 시작
            if (currentStamina == maxStamina)
            {
                lastStaminaUpdateTime = DateTime.Now;
            }
            
            currentStamina--;
            SaveStamina();
            Debug.Log("스테미너 1개 사용! 남은 스테미너: " + currentStamina);
            return true; // 사용 성공
        }
        else
        {
            Debug.Log("스테미너가 부족합니다!");
            return false; // 사용 실패
        }
    }

    // UI 업데이트
    void UpdateUI()
    {
        staminaText.text = currentStamina.ToString() + " / " + maxStamina.ToString();

        if (currentStamina < maxStamina)
        {
            TimeSpan timeToNextRecovery = TimeSpan.FromSeconds(recoveryDuration - (DateTime.Now - lastStaminaUpdateTime).TotalSeconds);
            timerText.text = string.Format("{0:00}:{1:00}", timeToNextRecovery.Minutes, timeToNextRecovery.Seconds);
        }
        else
        {
            timerText.text = "MAX";
        }
    }
}