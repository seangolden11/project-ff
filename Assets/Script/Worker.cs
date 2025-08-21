using PublicDataType;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;

public class Worker : MonoBehaviour
{

    public int jobType;

    Vector3 spot1;
    Vector3 spot2;

    private Vector3 targetPosition;
    private bool isWaiting = false; // 현재 대기 중인지 확인하는 변수

    void Start()
    {
        // switch (jobType)
        // {
        //     case DescType.Pump:
        //         break;
        //     case DescType.Guard:
        //         break;
        //     default:
        //         Building[] scripts = FindObjectsByType<Building>(FindObjectsSortMode.None);
        //         foreach (Building b in scripts)
        //         {
        //             if (b.buildinginfo.id == (int)(jobType - 3))
        //             {
        //                 spot1 = b.transform.parent.position;
        //             }
        //         }
        //         break;
        // }

        

    }


}
