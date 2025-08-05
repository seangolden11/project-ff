using PublicDataType;
using UnityEngine;

public class UpgradeBuilding : MonoBehaviour
{
    public BuildingInfo buildingInfo;
    public Building building;

    public GrassSpawner well;

    public Sell market;

    public BuildingType buildingType;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            switch (buildingType)
            {
                case BuildingType.Well:
                if (DataManager.Instance.GetUpgradeData(buildingInfo.id).level > well.level)
            {
                if (MoneyManager.Instance.TryRemoveMoney(buildingInfo.nextLeveCost[0]))
                {
                    // PrefabManager.Instance.Release(this.transform.parent.gameObject);
                    well.level++;
                    //업그레이드 로직
                }
            }
                    break;
                case BuildingType.Market:
                if (DataManager.Instance.GetUpgradeData(buildingInfo.id).level > market.level)
            {
                if (MoneyManager.Instance.TryRemoveMoney(buildingInfo.nextLeveCost[0]))
                {
                    // PrefabManager.Instance.Release(this.transform.parent.gameObject);
                    market.level++;
                    //업그레이드 로직
                }
            }
                    break;
                default:
                if (DataManager.Instance.GetUpgradeData(buildingInfo.id).level > building.level)
            {
                if (MoneyManager.Instance.TryRemoveMoney(buildingInfo.nextLeveCost[0]))
                {
                    // PrefabManager.Instance.Release(this.transform.parent.gameObject);
                    building.level++;
                    //업그레이드 로직
                }
            }
                    break;
            }
            
        }
    }
}
