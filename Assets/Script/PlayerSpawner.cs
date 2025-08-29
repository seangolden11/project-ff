using System.Collections.Generic;
using JetBrains.Annotations;
using PublicDataType;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public Transform wellspot;
    public Transform sellspot;
    public Transform[] animalSpot;
    public Transform[] spot;

    public Transform[] workerTrans;

    public Vector3 offset;

    public StageData stageData;
    public static List<Building> allBuildings = new List<Building>();
    [SerializeField]
    private List<Building> inspectorViewableBuildings;

    public Item[] items;

    bool[] isspawned = {false,false,false,false,false,false};



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        allBuildings.Clear();
        stageData = GameManager.Instance.stageData;
        List<EmpolyeeDatas> jd = DataManager.Instance.GetJobData();
        if (jd[1].job != 0)
        {
            PrefabManager.Instance.Get("Guard", Vector3.up, quaternion.identity).GetComponent<Guard>().patrolPoints.AddRange(spot);
        }

        GameObject well = PrefabManager.Instance.Get("well", wellspot.position + offset, wellspot.rotation);
        if (jd[2].job != 0)
        {
            GameObject temp = PrefabManager.Instance.Get("StandWorker", wellspot.position, wellspot.rotation);
            temp.GetComponentInChildren<TextMeshPro>().text = ((NameType)jd[2].name).ToString();
            temp.gameObject.GetComponent<CharacterModelChanger>().ChangeCharacterModel(jd[2].sprite);
        }
        GameObject sell = PrefabManager.Instance.Get("Sell", sellspot.position, quaternion.identity);
        



        for (int i = 0; i < stageData.animalSpawnInfo.ChickenNum; i++)
        {
            PrefabManager.Instance.Get("Chicken", animalSpot[0].position + new Vector3(0, 0.5f, 0), animalSpot[0].rotation);
        }
        for (int i = 0; i < stageData.animalSpawnInfo.SheepNum; i++)
        {
            PrefabManager.Instance.Get("Sheep", animalSpot[1].position, animalSpot[1].rotation);
        }
        for (int i = 0; i < stageData.animalSpawnInfo.CowNum; i++)
        {
            PrefabManager.Instance.Get("Cow", animalSpot[2].position, animalSpot[2].rotation);
        }



        for (int i = 0; i < stageData.buildingInfo.spawnBuilding.Count; i++)
        {
            if (stageData.buildingInfo.spawnBuilding[i] != null)
            {
                
                allBuildings.Add(PrefabManager.Instance.Get(stageData.buildingInfo.spawnBuilding[i].type.ToString(), spot[i].position, spot[i].rotation).GetComponentInChildren<Building>());
                if (jd[(int)stageData.buildingInfo.spawnBuilding[i].type + 2].job != 0 && !isspawned[(int)stageData.buildingInfo.spawnBuilding[i].type])
                {
                    GameObject tempobj = PrefabManager.Instance.Get("StandWorker", allBuildings[allBuildings.Count - 1].transform.position + offset, spot[i].rotation);
                    tempobj.GetComponent<CharacterModelChanger>().ChangeCharacterModel(jd[(int)stageData.buildingInfo.spawnBuilding[i].type + 2].sprite);
                    tempobj.GetComponentInChildren<TextMeshPro>().text = ((NameType)jd[(int)stageData.buildingInfo.spawnBuilding[i].type + 2].name).ToString();
                    isspawned[(int)stageData.buildingInfo.spawnBuilding[i].type] = true;
                }

            }

        }
        int j = 0;

        for (int i = jd.Count - 1; 8 < i; i--)
        {
            if (jd[i].rank != 0)
            {
                Worker worker = PrefabManager.Instance.Get("Worker", workerTrans[j].position, quaternion.identity).GetComponent<Worker>();
                worker.rank = jd[i].rank;
                worker.itemType = items[i - 9];
                worker.GetComponentInChildren<TextMeshPro>().text = ((NameType)jd[i].name).ToString();
                worker.gameObject.GetComponent<CharacterModelChanger>().ChangeCharacterModel(jd[i].sprite);
                worker.startTrans = workerTrans[j];
                j++;
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
