using System.Collections.Generic;
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
    Transform texTrans;

    
    void Start()
    {
        if (!textMeshPro)
            return;
        texTrans = textMeshPro.transform;
        switch (buildingType)
        {
            case BuildingType.Well:
                textMeshPro.text = $"${buildingInfo.nextLeveCost[well.level]}";

                break;
            case BuildingType.Market:
                textMeshPro.text = $"${buildingInfo.nextLeveCost[market.level]}";
                break;
            default:
                building = transform.parent.GetComponentInChildren<Building>();
                textMeshPro.text = $"${buildingInfo.nextLeveCost[building.level]}";
                texTrans.localEulerAngles = new Vector3(0,0,transform.parent.eulerAngles.y);
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
                            PlayerSpawner.allBuildings.Remove(building);
                            PlayerSpawner.allBuildings.Add(PrefabManager.Instance.Get(buildingInfo.nextLevelPrefab[building.level].name,this.transform.parent.position,this.transform.parent.rotation).GetComponentInChildren<Building>());
                            PrefabManager.Instance.Release(this.transform.parent.gameObject);

                        }
                    }
                    break;
            }

        }
    }

   
}
