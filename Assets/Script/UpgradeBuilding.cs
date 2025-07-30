using UnityEngine;

public class UpgradeBuilding : MonoBehaviour
{
    public BuildingInfo buildingInfo;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MoneyManager.Instance.TryRemoveMoney(buildingInfo.nextLevelCost))
            {
                PrefabManager.Instance.Release(this.transform.parent.gameObject);
                //업그레이드 로직
            }
        }
    }
}
