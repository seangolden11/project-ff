// StageData.cs (ScriptableObject 정의)
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStageData", menuName = "Game Data/Stage Data")]
public class StageData : ScriptableObject
{
    public int stageID;
    public float timeLimit;

    public BuildingSpawnInfo buildingInfo;
    public AnimalSpawnInfo animalSpawnInfo;
    public RewardInfo stageReward;
    public Goal goal;

    public enum Buildingtype { None, Powder }
    public enum Itemtype { Money, EggPowder,Egg,Chicken }

    // 필요한 다른 스테이지 관련 데이터 추가

    [System.Serializable]
    public class BuildingSpawnInfo
    {
        // 기존의 고정된 point1type, point1level 등을 제거하고 리스트로 대체합니다.
        public List<BuildingSpawnPoint> spawnBuilding; // 여기에 + 버튼이 생깁니다.
    }

    // 새로운 Serializable 클래스를 정의하여 enum과 int 쌍을 묶습니다.
    [System.Serializable]
    public class BuildingSpawnPoint
    {
        public Buildingtype type;
        public int level;
        
    }

    [System.Serializable]
    public class Goal
    {
        // 기존의 고정된 point1type, point1level 등을 제거하고 리스트로 대체합니다.
        public List<GoalItems> goalItems; // 여기에 + 버튼이 생깁니다.
    }

    [System.Serializable]
    public class GoalItems
    {
        public Itemtype type;
        public int count;
        
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