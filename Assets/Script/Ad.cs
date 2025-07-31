using TMPro;
using UnityEngine;

public class Well : MonoBehaviour
{
    public TextMeshPro gui;
    StageData sd;
    void Start()
    {
        gui = GetComponent<TextMeshPro>();
        sd = GameManager.Instance.stageData;
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < sd.goal.goalItems.Count; i++)
        {
            sb.Append($"{sd.goal.goalItems[i].type.ToString()} X {sd.goal.goalItems[i].count}");
        }

        gui.text = sb.ToString();
    }
}
