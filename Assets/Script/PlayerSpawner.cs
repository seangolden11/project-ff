using System.Collections.Generic;
using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public Transform wellspot;
    public Transform sellspot;
    public Transform animalSpot;
    public Transform[] spot;

    public StageData stageData;



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        GameObject well = PrefabManager.Instance.Get("well", wellspot.position,wellspot.rotation);
        GameObject sell = PrefabManager.Instance.Get("Sell", sellspot.position, quaternion.identity);



        for (int i = 0; i < stageData.animalSpawnInfo.ChickenNum; i++)
        {
            PrefabManager.Instance.Get("Chicken", animalSpot.position, animalSpot.rotation);
        }
        for (int i = 0; i < stageData.animalSpawnInfo.SheepNum; i++)
        {
            PrefabManager.Instance.Get("Sheep", animalSpot.position, animalSpot.rotation);
        }
        for (int i = 0; i < stageData.animalSpawnInfo.CowNum; i++)
        {
            PrefabManager.Instance.Get("Cow", animalSpot.position, animalSpot.rotation);
        }

        List<EmpolyeeDatas> jd = DataManager.Instance.GetJobData();
        
        for (int i = 0; i < stageData.buildingInfo.spawnBuilding.Count; i++)
        {
            if (stageData.buildingInfo.spawnBuilding[i] != null)
            {
                Vector3 tempPos = PrefabManager.Instance.Get(stageData.buildingInfo.spawnBuilding[i].type.ToString(), spot[i].position, spot[i].rotation).GetComponentInChildren<Building>().transform.position;
                if (jd[(int)stageData.buildingInfo.spawnBuilding[i].type + 3].job != 0)
                    PrefabManager.Instance.Get("Worker", tempPos, spot[i].rotation).GetComponent<Worker>().jobType = (int)stageData.buildingInfo.spawnBuilding[i].type;

            }

        }
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
