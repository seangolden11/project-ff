using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public GameObject[] contants;

    public UpgradeDataList upgradeDataList;

    // Update is called once per frame
    public void Init(int mode)
    {

        contants[mode].GetComponentInChildren<TextMeshProUGUI>().text = $"* : {upgradeDataList.upgradeList[DataManager.Instance.GetUpgradeData(mode).upgradeId].cost[DataManager.Instance.GetUpgradeData(mode).level]}";
        contants[mode].GetComponentInChildren<Image>().sprite = upgradeDataList.upgradeList[DataManager.Instance.GetUpgradeData(mode).upgradeId].sprite;

    }

    void Start()
    {
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
