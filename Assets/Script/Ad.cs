using System.Collections.Generic;
using PublicDataType;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ad : MonoBehaviour
{
    StageData sd;

    public GameObject prefab;

    List<GameObject> objects = new List<GameObject>();

    public List<Sprite> itemSprite;
    void Start()
    {
        sd = GameManager.Instance.stageData;

        for (int i = 0; i < sd.goal.goalItems.Count; i++)
        {
            objects.Add(Instantiate(prefab, transform));
            objects[i].GetComponentInChildren<TextMeshProUGUI>().text = $"0 / {sd.goal.goalItems[i].count}";
            objects[i].GetComponentsInChildren<Image>()[1].sprite = itemSprite[(int)sd.goal.goalItems[i].type];
        }

        
    }

    public void Init(ItemType it,int cur)
    {
        for (int i = 0; i < sd.goal.goalItems.Count; i++)
        {
            if (sd.goal.goalItems[i].type == it)
            {
                objects[i].GetComponentInChildren<TextMeshProUGUI>().text = $"{cur} / {sd.goal.goalItems[i].count}";
            }
        }
    }
}
