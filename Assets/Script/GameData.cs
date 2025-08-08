using System;
using System.Collections.Generic; // List를 사용하기 위해 필요

[Serializable] // Unity에서 이 클래스를 직렬화할 수 있도록 해줍니다.
public class StageClearData
{
    public int stageId;
    public bool cleared;
    public float clearTime; // 시간을 문자열로 저장 (예: "00:01:23")

    public int star;

    // 기본 생성자 (필수)
    public StageClearData()
    {
        stageId = 0;
        cleared = false;
        clearTime = 0;
        star = 0;
    }

    // 초기화 생성자
    public StageClearData(int id, bool isCleared, float time)
    {
        stageId = id;
        cleared = isCleared;
        clearTime = time;
        star = 0;
    }
}

[Serializable] // Unity에서 이 클래스를 직렬화할 수 있도록 해줍니다.
public class UpgradeData
{
    public int upgradeId;
    public int level;

    // 기본 생성자 (필수)
    public UpgradeData()
    {
        upgradeId = 0;
        level = 0;
    }

    // 초기화 생성자
    public UpgradeData(int id, int level = 0)
    {
        upgradeId = id;
        this.level = level;
    }
}

[Serializable] // Unity에서 이 클래스를 직렬화할 수 있도록 해줍니다.
public class GameData
{
    public List<StageClearData> stages;
    public List<UpgradeData> upgrades;
    public int stars;

    public int hearts;


    // 기본 생성자
    public GameData()
    {
        stages = new List<StageClearData>();
        upgrades = new List<UpgradeData>();
        stars = 0;
        hearts = 5;
    }
}