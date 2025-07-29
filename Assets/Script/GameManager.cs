using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics; // quaternion을 사용하기 위해 필요

public class GameManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static GameManager Instance { get; private set; }

    // 프리팹들을 담을 딕셔너리 (Inspector에서 수동 할당용)
    public StageList stageList;
    public StageData stageData;

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

    

    public void SetStage(int num)
    {
        stageData = stageList.allStages[num];
    }



}