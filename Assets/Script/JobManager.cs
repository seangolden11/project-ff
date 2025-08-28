using System;
using System.Collections.Generic;
using FGUIStarter;
using PublicDataType;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class JobManager : MonoBehaviour
{
    public GameObject hiredVeiw;
    public GameObject jobVeiw;

    public GameObject buttonPrefab;

    public List<Sprite> sprites;

    public List<GameObject> JobContents;
    public List<GameObject> HiredContents;


    public List<EmpolyeeDatas> jd;
    public List<EmpolyeeDatas> hd;

    bool isstarted = false;
    void Start()
    {
        Init();
        isstarted = true;
    }
    void OnEnable()
    {
        if (isstarted)
        {
            Init();
        }
    }

    void Init()
    {
        hd = DataManager.Instance.GetHiredData();
        jd = DataManager.Instance.GetJobData();

        if (jd.Count > 0 && JobContents.Count == 0)
        {
            for (int i = 0; i < jd.Count; i++)
            {
                GameObject tempbtn = Instantiate(buttonPrefab, jobVeiw.transform);

                tempbtn.GetComponentsInChildren<Image>()[1].sprite = sprites[jd[i].sprite];
                tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[0].text = ((RankType)jd[i].rank).ToString();
                tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[1].text = ((NameType)jd[i].name).ToString();
                tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"job {((DescType)jd[i].job).ToString()} has {(jd[i].rank) * 25}% more effcient";
                tempbtn.GetComponent<CustomButton>().id = i;
                tempbtn.GetComponent<CustomButton>().mode = 0;

                JobContents.Add(tempbtn);
            }
        }
        else if (JobContents.Count > 0)
        {
            for (int i = 0; i < jd.Count; i++)
            {
                GameObject tempbtn = JobContents[i];

                tempbtn.GetComponentsInChildren<Image>()[1].sprite = sprites[jd[i].sprite];
                tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[0].text = ((RankType)jd[i].rank).ToString();
                tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[1].text = ((NameType)jd[i].name).ToString();
                tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"job {((DescType)jd[i].job).ToString()} has {(jd[i].rank) * 25}% more effcient";


            }
        }

        if (hd.Count > 0 && HiredContents.Count == 0)
        {

            foreach (EmpolyeeDatas temp in hd)
            {
                InitHired(temp);
            }
        }
        else if(HiredContents.Count > 0)
        {
            foreach (GameObject temp in HiredContents)
            {
                Destroy(temp);
            }
            HiredContents.Clear();
            foreach (EmpolyeeDatas temp in hd)
            {
                InitHired(temp);
            }
        }
    }

    void InitJob()
    {
        for (int i = 0; i < jd.Count; i++)
        {
            JobContents[i].GetComponentsInChildren<Image>()[1].sprite = sprites[jd[i].sprite];
            JobContents[i].GetComponentsInChildren<TextMeshProUGUI>()[0].text = ((RankType)jd[i].rank).ToString();
            JobContents[i].GetComponentsInChildren<TextMeshProUGUI>()[1].text = ((NameType)jd[i].name).ToString();
            JobContents[i].GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"job {((DescType)jd[i].job).ToString()} has {(jd[i].rank) * 25}% more effcient";
        }
    }

    void InitHired(EmpolyeeDatas temp)
    {
        GameObject tempbtn = Instantiate(buttonPrefab, hiredVeiw.transform);
        tempbtn.GetComponentsInChildren<Image>()[1].sprite = sprites[temp.sprite];
        tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[0].text = ((RankType)temp.rank).ToString();
        tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[1].text = ((NameType)temp.name).ToString();
        tempbtn.GetComponentsInChildren<TextMeshProUGUI>()[2].text = $"job {((DescType)temp.job).ToString()} has {(temp.rank) * 25}% more effcient";
        tempbtn.GetComponent<CustomButton>().id = HiredContents.Count;
        tempbtn.GetComponent<CustomButton>().mode = 1;

        HiredContents.Add(tempbtn);

        if (temp.isAssigned)
        {
            tempbtn.SetActive(false);
        }
    }

    public void InitID()
    {
        for (int i = 0; i < HiredContents.Count; i++)
        {
            HiredContents[i].GetComponent<CustomButton>().id = i;
        }
    }

    public void EmployeeJobGranted(int id)
    {
        if (jd[hd[id].job - 1].job == hd[id].job)
            return;
        hd[id].isAssigned = true;
        jd[hd[id].job].originalHiredIndex = id;

        jd[hd[id].job].rank = hd[id].rank;
        jd[hd[id].job].name = hd[id].name;
        jd[hd[id].job].job = hd[id].job;
        jd[hd[id].job].sprite = hd[id].sprite;
        hd[id].originalHiredIndex = hd[id].job;

        HiredContents[id].SetActive(false);
        InitJob();
        DataManager.Instance.SetJob(jd, hd);

    }

    public void EmployeeJobCancel(int id)
    {
        if (jd[id].rank == 0)
            return;
        else
        {
            HiredContents[jd[id].originalHiredIndex].SetActive(true);
            hd[jd[id].originalHiredIndex].isAssigned = false;
            jd[id].rank = 0;
            jd[id].job = 0;
            jd[id].name = 0;
            jd[id].sprite = 0;
            InitJob();
            DataManager.Instance.SetJob(jd, hd);        
        }
    }

    

    public void ButtonPress(int id, int mode)
    {
        if (mode == 1)
        {
            EmployeeJobGranted(id);
            Debug.Log(id);
        }
        else
        {
            EmployeeJobCancel(id);
        }
    }

    
}
