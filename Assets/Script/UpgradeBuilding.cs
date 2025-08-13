using PublicDataType;
using TMPro;
using UnityEngine;

public class UpgradeBuilding : MonoBehaviour
{
    public BuildingInfo buildingInfo;
    public Building building;

    public GrassSpawner well;

    public Sell market;

    public BuildingType buildingType;

    public TextMeshPro textMeshPro;
    void Start()
    {
        if (!textMeshPro)
            return;
        switch (buildingType)
        {
            case BuildingType.Well:
                textMeshPro.text = $"${buildingInfo.nextLeveCost[well.level]}";
                break;
            case BuildingType.Market:
                textMeshPro.text = $"${buildingInfo.nextLeveCost[market.level]}";
                break;
            default:
                textMeshPro.text = $"${buildingInfo.nextLeveCost[building.level]}";
                break;
        }
        

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (buildingType)
            {
                case BuildingType.Well:
                     if (DataManager.Instance.GetUpgradeData(buildingInfo.id).level > well.level)
                    {
                        if (MoneyManager.Instance.TryRemoveMoney(buildingInfo.nextLeveCost[well.level]))
                        {
                            PrefabManager.Instance.Get(buildingInfo.nextLevelPrefab[well.level].name,this.transform.parent.parent.position,this.transform.parent.parent.rotation);
                            PrefabManager.Instance.Release(this.transform.parent.parent.gameObject);
                            //업그레이드 로직
                        }
                    }
                    break;
                case BuildingType.Market:
                    if (DataManager.Instance.GetUpgradeData(buildingInfo.id).level > market.level)
                    {
                        if (MoneyManager.Instance.TryRemoveMoney(buildingInfo.nextLeveCost[market.level]))
                        {
                            PrefabManager.Instance.Get(buildingInfo.nextLevelPrefab[market.level].name,this.transform.parent.parent.position,this.transform.parent.parent.rotation);
                            PrefabManager.Instance.Release(this.transform.parent.parent.gameObject);
                            //업그레이드 로직
                        }
                    }
                    break;
                default:
                    if (DataManager.Instance.GetUpgradeData(buildingInfo.id).level > building.level)
                    {
                        if (MoneyManager.Instance.TryRemoveMoney(buildingInfo.nextLeveCost[building.level]))
                        {
                            PrefabManager.Instance.Get(buildingInfo.nextLevelPrefab[building.level].name,this.transform.parent.position,this.transform.parent.rotation);
                            PrefabManager.Instance.Release(this.transform.parent.gameObject);

                        }
                    }
                    break;
            }

        }
    }

   
}
