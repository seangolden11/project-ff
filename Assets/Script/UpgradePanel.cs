using System.Collections.Generic;
using FGUIStarter;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public List<GameObject> contants;
    public List<Sprite> sp;

    public GameObject buttonPrefab;
    public Transform contantTransform;

    public UpgradeDataList upgradeDataList;

    // Update is called once per frame
    public void Init(int mode)
    {

        contants[mode].GetComponentsInChildren<TextMeshProUGUI>()[0].text = $"{upgradeDataList.upgradeList[mode].name}";
        contants[mode].GetComponentsInChildren<TextMeshProUGUI>()[1].text = $"* : {upgradeDataList.upgradeList[DataManager.Instance.GetUpgradeData(mode).upgradeId].cost[DataManager.Instance.GetUpgradeData(mode).level - 1]}";
        contants[mode].GetComponentsInChildren<Image>()[2].sprite = upgradeDataList.upgradeList[DataManager.Instance.GetUpgradeData(mode).upgradeId].sprite;
        contants[mode].GetComponentsInChildren<Image>()[1].sprite = sp[DataManager.Instance.GetUpgradeData(mode).level];


    }

    void Start()
    {
        for (int i = 0; i < upgradeDataList.upgradeList.Count; i++)
        {
            GameObject temp = Instantiate(buttonPrefab, contantTransform);
            temp.GetComponent<CustomButton>().id = i;
            contants.Add(temp);
        }

        for (int i = 0; i < upgradeDataList.upgradeList.Count; i++)
        {
            Init(i);
        }
    }

    public void Upgrade(int id)
    {
        Debug.Log(id);
        DataManager.Instance.SetUpgradeData(id, 1);
        Init(id);
    }


}
