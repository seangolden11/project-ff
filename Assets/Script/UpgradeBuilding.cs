using UnityEngine;

public class UpgradeBuilding : MonoBehaviour
{
    public BuildingInfo buildingInfo;
    public Building building;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MoneyManager.Instance.TryRemoveMoney(buildingInfo.nextLeveCost[0]))
            {
                // PrefabManager.Instance.Release(this.transform.parent.gameObject);
                building.level++;
                //업그레이드 로직
            }
        }
    }
}
