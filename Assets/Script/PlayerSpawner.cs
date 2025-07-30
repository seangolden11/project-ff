using JetBrains.Annotations;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public Transform wellspot;
    public Transform sellspot;
    public Transform animalSpot;
    public int stageNum;
    public Transform spot1;

    public StageList stageList;



    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        GameObject well = PrefabManager.Instance.Get("well", wellspot.position,quaternion.identity);
        GameObject sell = PrefabManager.Instance.Get("Sell", sellspot.position, quaternion.identity);



        for (int i = 0; i < stageList.allStages[stageNum].animalSpawnInfo.ChickenNum; i++)
        {
            PrefabManager.Instance.Get("Chicken", animalSpot.position, animalSpot.rotation);
        }


        
        PrefabManager.Instance.Get(stageList.allStages[stageNum].buildingInfo.spawnBuilding[0].type.ToString(), spot1.position,quaternion.identity);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
