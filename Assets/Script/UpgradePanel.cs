using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public List<GameObject> contants;

    public GameObject buttonPrefab;
    public Transform contantTransform;

    public UpgradeDataList upgradeDataList;

    // Update is called once per frame
    public void Init(int mode)
    {

        contants[mode].GetComponentsInChildren<TextMeshProUGUI>()[0].text = $"{upgradeDataList.upgradeList[mode].name}";
        contants[mode].GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"* : {upgradeDataList.upgradeList[DataManager.Instance.GetUpgradeData(mode).upgradeId].cost[DataManager.Instance.GetUpgradeData(mode).level]}";
        contants[mode].GetComponentsInChildren<Image>()[1].sprite = upgradeDataList.upgradeList[DataManager.Instance.GetUpgradeData(mode).upgradeId].sprite;

    }

    void Start()
    {
        for (int i = 0; i < upgradeDataList.upgradeList.Count; i++)
        {
            GameObject temp = Instantiate(buttonPrefab, contantTransform);
            contants.Add(temp);
        }

        for (int i = 0; i < upgradeDataList.upgradeList.Count; i++)
        {
            Init(i);
        }
    }

    public void Upgrade(int id)
    {
        DataManager.Instance.SetUpgradeData(id, 1);
        Init(id);
    }


}
