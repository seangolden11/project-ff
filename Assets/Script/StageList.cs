// StageList.cs
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewStageList", menuName = "Game Data/Stage List")]
public class StageList : ScriptableObject
{
    public List<StageData> allStages; // StageData ScriptableObject들을 담을 리스트
}