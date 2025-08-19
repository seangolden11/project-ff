using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StagePanel : MonoBehaviour
{
    public StageList stageList;
    public int stageNum = 0;


    public TextMeshProUGUI stagenumgui;
    public TextMeshProUGUI stagoal1gui;
    public TextMeshProUGUI stagoal2gui;
    public TextMeshProUGUI stagoal3gui;
    public TextMeshProUGUI stagoal4gui;
    public TextMeshProUGUI rewardgui;
    public TextMeshProUGUI goalgui;

    float clearTime;

    public Slider slider;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init(0);
    }

    // Update is called once per frame
    public void Init(int mode)
    {
        if ((stageNum + mode == -1) || stageNum + mode >= stageList.allStages.Count)
            return;
        stageNum += mode;
        GameManager.Instance.SetStage(stageNum);
        stagoal1gui.text = $"{stageList.allStages[stageNum].timeLimit}";
        stagoal2gui.text = $"{stageList.allStages[stageNum].timeLimit + 120f}";
        stagoal3gui.text = $"{stageList.allStages[stageNum].timeLimit + 240f}";
        stagoal4gui.text = $"{stageList.allStages[stageNum].timeLimit + 360f}";
        stagenumgui.text = $"Stage : {stageNum + 1}";


        Timeinit();
        

        if (stageList.allStages[stageNum].stageReward != null)
            rewardgui.text = $"{stageList.allStages[stageNum].stageReward}";
        rewardgui.text = "Star";

        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        for (int i = 0; i < stageList.allStages[stageNum].goal.goalItems.Count; i++)
        {
            sb.Append($"{stageList.allStages[stageNum].goal.goalItems[i].type.ToString()} X {stageList.allStages[stageNum].goal.goalItems[i].count}");
        }

        goalgui.text = sb.ToString();




    }

    public void Timeinit()
    {
        
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {

            TimerManager tm = FindFirstObjectByType<TimerManager>();
            slider.value = ((float)(GameManager.Instance.stageData.timeLimit + 360) - tm.currentGameTime) / (GameManager.Instance.stageData.timeLimit + 360);
        }
        else
        {
            if (DataManager.Instance.GetStageData(stageNum).clearTime != 0)
        {
            slider.value = (stageList.allStages[stageNum].timeLimit + 360f - DataManager.Instance.GetStageData(stageNum).clearTime) / (stageList.allStages[stageNum].timeLimit + 360f);
        }
        else
        {
            slider.value = 1;
        }
        }
    }
}
