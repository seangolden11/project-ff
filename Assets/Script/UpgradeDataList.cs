using UnityEngine;
using System.Collections.Generic;
using System;

[CreateAssetMenu(fileName = "NewBuildingUpgradeData", menuName = "Game Data/BuildingUpgradeData")]
public class UpgradeDataList : ScriptableObject
{
    public List<BuildingUpgradeData> upgradeList;
}
[Serializable]
public class BuildingUpgradeData
{
    public int Id;
    public Sprite sprite;

    public int[] cost;
}

