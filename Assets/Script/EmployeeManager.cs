using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmployeeManager : MonoBehaviour
{
    public GameObject hiredVeiw;
    public GameObject hireVeiw;

    public GameObject buttonPrefab;

    public int rollnum;

    public List<Sprite> sprites;

    public List<GameObject> HireContents;
    public List<GameObject> HiredContents;
    public List<String> nameList;
    public List<String> rankList;

    public List<String> descList;


    public List<EmpolyeeDatas> ed;
    public List<EmpolyeeDatas> hd;
    void Start()
    {
        ed = DataManager.Instance.GetEmployeeData();
        hd = DataManager.Instance.GetHiredData();

        if (ed.Count > 0)
        {
            foreach (EmpolyeeDatas temp in ed)
            {
                InitHire(temp);
            }
        }
        else
        {
            for (int i = 0; i < rollnum; i++)
            {
                
                int tempspirte = UnityEngine.Random.Range(0, sprites.Count);
                int tempname = UnityEngine.Random.Range(0, sprites.Count);
                int tempjob = UnityEngine.Random.Range(0, sprites.Count);
                int temprank = UnityEngine.Random.Range(0, sprites.Count);
                ed.Add(new EmpolyeeDatas(tempname, temprank, tempjob, tempspirte));
                InitHire(ed[i]);

                
            }
        }
        if (hd.Count > 0)
        {
            foreach (EmpolyeeDatas temp in hd)
            {
                InitHired(temp);
            }
        }
    }

    void InitHire(EmpolyeeDatas temp)
    {
        GameObject tempbtn = Instantiate(buttonPrefab, hireVeiw.transform);
                tempbtn.GetComponentsInChildren<Image>()[1].sprite = sprites[temp.sprite];
                tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[0].text = rankList[temp.rank];
                tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[1].text = nameList[temp.name];
                tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"job {descList[temp.job]} has {temp.rank * 25}% more effcient";

                HireContents.Add(tempbtn);
    }

    void InitHired(EmpolyeeDatas temp)
    {
        GameObject tempbtn = Instantiate(buttonPrefab, hiredVeiw.transform);
        tempbtn.GetComponentsInChildren<Image>()[1].sprite = sprites[temp.sprite];
        tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[0].text = rankList[temp.rank];
        tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[1].text = nameList[temp.name];
        tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"job {descList[temp.job]} has {temp.rank * 25}% more effcient";

        HiredContents.Add(tempbtn);
    }
}
