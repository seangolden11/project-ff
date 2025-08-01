using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
using System;
using System.Timers; // quaternion을 사용하기 위해 필요

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    // 프리팹들을 담을 딕셔너리 (Inspector에서 수동 할당용)
    public StageList stageList;
    public StageData stageData;
    public TimerUI timer;
    public TimerManager tm;

    public int Stars;

    private void Awake()
    {
        // 싱글톤 패턴 구현
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않도록
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        

    }

    public void OnStageStart()
    {
        if (MoneyManager.Instance)
        {
            timer = GameObject.Find("Timer").GetComponent<TimerUI>();
            tm = FindFirstObjectByType<TimerManager>();

            MoneyManager.Instance.AddMoney(stageData.startMoney);
        }
    }




    public void SetStage(int num)
    {
        stageData = stageList.allStages[num];
    }

    public void StageClear()
    {
        tm.StopTimer();
        UIManager.Instance.ToggleStagePanel();
        
        float count = 0;
        if (tm.starCount == 1)
            count += stageData.timeLimit;
        else if (tm.starCount == 2)
            count += 120;
        else if (tm.starCount == 3)
            count += 240;

        count += stageData.timeLimit - tm.currentTime;

        DataManager.Instance.SetStageCleared(stageData.stageID, count, 3 - tm.starCount);
    }



}