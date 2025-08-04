using TMPro;
using UnityEngine;
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

    void OnEnable()
    {
        if (slider)
        {
            TimerManager tm = FindFirstObjectByType<TimerManager>();
            float count = 0;
            if (tm.starCount == 1)
                count += GameManager.Instance.stageData.timeLimit;
            else if (tm.starCount == 2)
                count += GameManager.Instance.stageData.timeLimit + 120;
            else if (tm.starCount == 3)
                count += GameManager.Instance.stageData.timeLimit + 240;
            slider.value = (GameManager.Instance.stageData.timeLimit - tm.currentTime + count) / (GameManager.Instance.stageData.timeLimit + 4 * 120);
        }
    }
}
