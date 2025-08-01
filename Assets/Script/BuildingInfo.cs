using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// 이 스크립트는 건물 정보의 구조를 정의합니다.

[CreateAssetMenu(fileName = "NewBuildingData", menuName = "Game Data/Building Data")]
public class BuildingInfo : ScriptableObject
{
    public int level;

    public int id;
    public enum Buildingtype { None, Powder };
    public Item itemTakes;
    public Item itemGives;

    public GameObject nextLevelPrefab;
    public List<int> nextLeveCost;
}

