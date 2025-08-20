using System;
using System.Collections.Generic;
using FGUIStarter;
using PublicDataType;
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
        tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[0].text = ((RankType)temp.rank).ToString();
        tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[1].text = ((NameType)temp.name).ToString();
        tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"job {((DescType)temp.job).ToString()} has {(temp.rank+1) * 25}% more effcient";
        tempbtn.GetComponent<CustomButton>().id = HireContents.Count;
        tempbtn.GetComponent<CustomButton>().mode = 1;

        HireContents.Add(tempbtn);
    }

    void InitHired(EmpolyeeDatas temp)
    {
        GameObject tempbtn = Instantiate(buttonPrefab, hiredVeiw.transform);
        tempbtn.GetComponentsInChildren<Image>()[1].sprite = sprites[temp.sprite];
        tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[0].text = ((RankType)temp.rank).ToString();
        tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[1].text = ((NameType)temp.name).ToString();
        tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"job {((DescType)temp.job).ToString()} has {(temp.rank+1) * 25}% more effcient";
        tempbtn.GetComponent<CustomButton>().id = HiredContents.Count;
        tempbtn.GetComponent<CustomButton>().mode = 0;

        HiredContents.Add(tempbtn);
    }

    public void InitID()
    {
        for (int i = 0; i < HireContents.Count; i++)
        {
            HireContents[i].GetComponent<CustomButton>().id = i;
        }
        for (int i = 0; i < HiredContents.Count; i++)
        {
            HiredContents[i].GetComponent<CustomButton>().id = i;
        }
    }

    public void HireEmployee(int id)
    {
        if (DataManager.Instance.TryUseStar(ed[id].rank+1))
        {
            HireContents[id].GetComponent<CustomButton>().mode = 0;

            HireContents[id].transform.SetParent(hiredVeiw.transform, false);
            HiredContents.Add(HireContents[id]);
            hd.Add(ed[id]);
            HireContents.Remove(HireContents[id]);
            ed.Remove(ed[id]);
            InitID();

            DataManager.Instance.SetEmployee(ed, hd);

        }

    }

    public void FireEmployee(int id)
    {
        DataManager.Instance.PlusStar(hd[id].rank+1);
        HiredContents[id].GetComponent<CustomButton>().mode = 1;
        HiredContents[id].GetComponent<CustomButton>().id = HireContents.Count;
        HiredContents[id].transform.SetParent(hireVeiw.transform, false);
        HireContents.Add(HiredContents[id]);
        ed.Add(hd[id]);
        HiredContents.Remove(HiredContents[id]);
        hd.Remove(hd[id]);
        InitID();
        DataManager.Instance.SetEmployee(ed, hd);

    }

    public void Reroll()
    {
        if (DataManager.Instance.TryUseStar(1))
        {
            ed.Clear();
            foreach (GameObject go in HireContents)
            {
                Destroy(go);
            }
            HireContents.Clear();
            for (int i = 0; i < rollnum; i++)
            {

                int tempspirte = UnityEngine.Random.Range(0, sprites.Count);
                int tempname = UnityEngine.Random.Range(0, sprites.Count);
                int tempjob = UnityEngine.Random.Range(0, sprites.Count);
                int temprank = UnityEngine.Random.Range(0, sprites.Count);
                ed.Add(new EmpolyeeDatas(tempname, temprank, tempjob, tempspirte));
                InitHire(ed[i]);


            }

            DataManager.Instance.SetEmployee(ed, hd);

        }
    }

    public void ButtonPress(int id, int mode)
    {
        if (mode == 1)
        {
            HireEmployee(id);
            Debug.Log(id);
        }
        else
        {
            FireEmployee(id);
        }
    }

    
}
