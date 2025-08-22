using System.Collections.Generic;
using JetBrains.Annotations;
using PublicDataType;
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
    public static List<Building> allBuildings = new List<Building>();
    [SerializeField]
    private List<Building> inspectorViewableBuildings;

    public Item[] items;



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        List<EmpolyeeDatas> jd = DataManager.Instance.GetJobData();
        if (jd[1].job != 0)
        {
            PrefabManager.Instance.Get("Guard", Vector3.up, quaternion.identity).GetComponent<Guard>().patrolPoints.AddRange(spot);
        }

        GameObject well = PrefabManager.Instance.Get("well", wellspot.position, wellspot.rotation);
        if (jd[2].job != 0)
            PrefabManager.Instance.Get("StandWorker", wellspot.position, wellspot.rotation);
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



        for (int i = 0; i < stageData.buildingInfo.spawnBuilding.Count; i++)
        {
            if (stageData.buildingInfo.spawnBuilding[i] != null)
            {
                
                allBuildings.Add(PrefabManager.Instance.Get(stageData.buildingInfo.spawnBuilding[i].type.ToString(), spot[i].position, spot[i].rotation).GetComponentInChildren<Building>());
                if (jd[(int)stageData.buildingInfo.spawnBuilding[i].type + 2].job != 0)
                    PrefabManager.Instance.Get("StandWorker", allBuildings[allBuildings.Count - 1].transform.position, spot[i].rotation);

            }

        }

        for (int i = jd.Count - 1; 8 < i; i--)
        {
            if (jd[i].rank != 0)
            {
                Worker worker = PrefabManager.Instance.Get("Worker", Vector3.up, quaternion.identity).GetComponent<Worker>();
                worker.rank = jd[i].rank;
                worker.itemType = items[i - 9];
            }


        }


    }

    void Update()
    {
        #if UNITY_EDITOR
        inspectorViewableBuildings = allBuildings;
        #endif
    }
}
