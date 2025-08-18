using System;
using System.Collections.Generic;
using UnityEngine;

public class EmployeeManager : MonoBehaviour
{
    public GameObject hiredVeiw;
    public GameObject hireVeiw;

    public List<Sprite> sprites;

    public List<GameObject> conTents;
    public List<String> nameList;

    public class EmpolyeeData
    {
        int nameNum;
        int job;
        int rankType;
        int spriteNum;
    }
    void Start()
    {
        
    }
}
