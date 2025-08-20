using System;
using System.Collections.Generic;
using NUnit.Framework.Constraints; // List를 사용하기 위해 필요

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
        level = 1;
    }

    // 초기화 생성자
    public UpgradeData(int id, int level = 1)
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

    public List<EmpolyeeDatas> empdata;
    public List<EmpolyeeDatas> hiredata;
    public List<EmpolyeeDatas> Jobdata;
    public int stars;

    public int hearts;

    public string recoverTime;


    // 기본 생성자
    public GameData()
    {
        stages = new List<StageClearData>();
        upgrades = new List<UpgradeData>();
        empdata = new List<EmpolyeeDatas>();
        hiredata = new List<EmpolyeeDatas>();
        Jobdata = new List<EmpolyeeDatas>();
        stars = 0;
        hearts = 5;
    }
}

[Serializable] // Unity에서 이 클래스를 직렬화할 수 있도록 해줍니다.
public class EmpolyeeDatas
{
    public int name;

    public int rank;
    public int job;

    public int sprite;
    public int originalHiredIndex;
    public bool isAssigned;


    // 기본 생성자
    public EmpolyeeDatas()
    {
        name = 0;
        rank = 0;
        job = 0;
        sprite = 0;
        originalHiredIndex = -1;
        isAssigned = false;
    }

    public EmpolyeeDatas(int name, int rank, int job, int sprite)
    {
        this.name = name;
        this.rank = rank;
        this.job = job;
        this.sprite = sprite;
        originalHiredIndex = -1;
        isAssigned = false;
    }
}