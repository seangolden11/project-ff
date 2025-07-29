using JetBrains.Annotations;
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
        GameObject well = PrefabManager.Instance.InstantiatePrefab("well", wellspot);
        GameObject sell = PrefabManager.Instance.InstantiatePrefab("Sell", sellspot);



        for (int i = 0; i < stageList.allStages[stageNum].animalSpawnInfo.ChickenNum; i++)
        {
            PrefabManager.Instance.InstantiatePrefab("Chicken", animalSpot.position, animalSpot.rotation);
        }


        
        PrefabManager.Instance.InstantiatePrefab(stageList.allStages[stageNum].buildingInfo.spawnBuilding[0].type.ToString(), spot1);
        
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
