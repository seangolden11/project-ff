// StageData.cs (ScriptableObject 정의)
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStageData", menuName = "Game Data/Stage Data")]

public class StageData : ScriptableObject
{
    public int stageID;
    // public string stageName;
    public float timeLimit;

    public BuildingSpawnInfo buildingInfo;
    public AnimalSpawnInfo animalSpawnInfo;
    public RewardInfo stageReward; // 스테이지 클리어 보상 정보

    public enum Buildingtype { None, Powder }

    // 필요한 다른 스테이지 관련 데이터 추가

    [System.Serializable]
    public class BuildingSpawnInfo
    {
        public Buildingtype point1type;
        public int point1level;

        public Buildingtype point2type;
        public int point2level;

        public Buildingtype point3type;
        public int point3level;

        public Buildingtype point4type;
        public int point4level;

        public Buildingtype point5type;
        public int point5level;

        public Buildingtype point6type;
        public int point6level;
    }

    [System.Serializable]
    public class AnimalSpawnInfo
    {
        public int ChickenNum;
        public int SheepNum;
        public int CowNum;
    }

    [System.Serializable]
    public class RewardInfo
    {
        public int gold;
        public int experience;
        // public List<ItemData> items; // ItemData는 별도의 ScriptableObject 또는 Serializable 클래스일 수 있음
    }
}