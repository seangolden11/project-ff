using TMPro;
using UnityEngine;

public class StagePanel : MonoBehaviour
{
    public StageList stageList;
    public int stageNum;
    

    public TextMeshProUGUI stagenumgui;
    public TextMeshProUGUI stagoal1gui;
    public TextMeshProUGUI stagoal2gui;
    public TextMeshProUGUI stagoal3gui;
    public TextMeshProUGUI stagoal4gui;
    public TextMeshProUGUI rewardgui;
    public TextMeshProUGUI goalgui;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Init(0);
    }

    // Update is called once per frame
    public void Init(int mode)
    {
        stageNum += mode;

        stagoal1gui.text = $"{stageList.allStages[stageNum].timeLimit}";
        stagoal2gui.text = $"{stageList.allStages[stageNum].timeLimit + 120f}";
        stagoal3gui.text = $"{stageList.allStages[stageNum].timeLimit + 240f}";
        stagoal4gui.text = $"{stageList.allStages[stageNum].timeLimit + 360f}";
        stagenumgui.text = $"Stage : {stageNum + 1}";

        if (stageList.allStages[stageNum].stageReward != null)
            rewardgui.text = $"{stageList.allStages[stageNum].stageReward}";
        rewardgui.text = "Star";

        goalgui.text = "temp string";



        
    }
}
