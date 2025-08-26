using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItem", menuName = "Game Data/Models")]
public class WorkerModel : ScriptableObject
{
    public List<GameObject> models;
}