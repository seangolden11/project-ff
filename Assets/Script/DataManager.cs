using UnityEngine;
using System.IO; // 파일 입출력을 위해 필요
using System.Collections.Generic; // List를 위해 필요

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; } // 싱글톤 패턴 (옵션)

    private GameData gameData;
    private string saveFilePath; // 저장될 파일 경로

    private void Awake()
    {
        // 싱글톤 패턴 구현 (선택 사항이지만 편리합니다)
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 오브젝트가 파괴되지 않도록
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // 저장 파일 경로 설정: Application.persistentDataPath는 플랫폼별로 영구 데이터를 저장하기에 적합한 경로를 반환합니다.
        saveFilePath = Path.Combine(Application.persistentDataPath, "game_progress.json");
        Debug.Log($"저장 파일 경로: {saveFilePath}");


        LoadGameProgress(); // 게임 시작 시 데이터 로드
    }

    // 게임 진행 데이터 로드
    public void LoadGameProgress()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                string jsonString = File.ReadAllText(saveFilePath);
                gameData = JsonUtility.FromJson<GameData>(jsonString);
                Debug.Log($"게임 진행 데이터를 성공적으로 로드했습니다.{gameData.stages}");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"게임 진행 데이터 로드 중 오류 발생: {e.Message}");
                InitializeNewGameProgress(); // 오류 발생 시 새 데이터 초기화
            }
        }
        else
        {
            Debug.Log("저장 파일이 존재하지 않습니다. 새 게임 진행 데이터를 초기화합니다.");
            InitializeNewGameProgress();
        }
    }

    // 게임 진행 데이터 저장
    public void SaveGameProgress()
    {
        try
        {
            string jsonString = JsonUtility.ToJson(gameData, true); // true는 가독성 좋게 들여쓰기
            File.WriteAllText(saveFilePath, jsonString);
            Debug.Log("게임 진행 데이터를 성공적으로 저장했습니다.");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"게임 진행 데이터 저장 중 오류 발생: {e.Message}");
        }
    }

    // 새 게임 진행 데이터 초기화 (40개 스테이지 생성)
    private void InitializeNewGameProgress()
    {
        gameData = new GameData();
        for (int i = 0; i < 40; i++)
        {
            gameData.stages.Add(new StageClearData(i, false, 0f)); // 초기에는 클리어 안 됨, 시간 없음
        }
        for (int i = 0; i < 10; i++)
        {
            gameData.upgrades.Add(new UpgradeData(i, 1)); // 초기에는 클리어 안 됨, 시간 없음
        }
        SaveGameProgress(); // 초기화 후 즉시 저장 (선택 사항)
    }

    public StageClearData GetStageData(int stageId)
    {

        return gameData.stages[stageId];
    }

    public int GetStarData()
    {

        return gameData.stars;
    }

    public int GetHeartData()
    {

        return gameData.hearts;
    }

    public string GetrecoverData()
    {
        return gameData.recoverTime;
    }
    public List<EmpolyeeDatas> GetHiredData()
    {
        return gameData.hiredata;
    }

    public List<EmpolyeeDatas> GetEmployeeData()
    {
        return gameData.empdata;
    }

    

    public UpgradeData GetUpgradeData(int Id)
    {

        return gameData.upgrades[Id];
    }

    public void SetrecoverData(string num, int heartsnum)
    {
        gameData.recoverTime = num;
        gameData.hearts = heartsnum;
    }

    // 특정 스테이지의 클리어 상태 업데이트
    public void SetStageCleared(int stageId, float clearTime, int num)
    {
        StageClearData stage = GetStageData(stageId);
        if (stage != null)
        {
            if (stage.cleared == true)
            {
                if (stage.clearTime > clearTime)
                {
                    stage.clearTime = clearTime;
                    gameData.stars += num - stage.star;
                    stage.star = num;
                }
            }
            else
            {
                stage.cleared = true;
                stage.clearTime = clearTime;
                stage.star = num;
                gameData.stars += num;
            }

            Debug.Log($"스테이지 {stageId} 클리어 정보 업데이트: 클리어됨={stage.cleared}, 시간={stage.clearTime}");
            SaveGameProgress(); // 변경사항 저장
        }
        else
        {
            Debug.LogWarning($"스테이지 ID {stageId}를 찾을 수 없습니다.");
        }
    }

    public void SetUpgradeData(int Id, int num)
    {
        UpgradeData upgrade = GetUpgradeData(Id);
        if (upgrade != null)
        {
            if (upgrade.level == 5 || gameData.stars < num)
                return;
            upgrade.level++;
            gameData.stars -= num;
            Debug.Log($"업그레이드 {Id} 정보 업데이트: 레벨={upgrade.level}");
            SaveGameProgress(); // 변경사항 저장
        }
        else
        {
            Debug.LogWarning($"스테이지 ID {upgrade}를 찾을 수 없습니다.");
        }
    }
    
    public void DeleteAndResetData()
    {
        if (File.Exists(saveFilePath))
        {
            try
            {
                File.Delete(saveFilePath);
                Debug.Log("Save file successfully deleted.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Error deleting save file: {e.Message}");
            }
        }
        else
        {
            Debug.LogWarning("Save file to delete does not exist.");
        }

        // Initialize new data in memory and save it
        Debug.Log("Resetting game data to initial state.");
        InitializeNewGameProgress();
    }
    

}
