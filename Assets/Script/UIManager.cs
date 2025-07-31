using UnityEngine;

public class UIManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static UIManager Instance { get; private set; }

    [SerializeField] // 인스펙터에서 할당할 수 있도록 SerializeField 사용
    private GameObject settingsPanel; 
    [SerializeField]
    private GameObject stagePanel;

    void Awake()
    {
        // 싱글톤 인스턴스 설정
        if (Instance == null)
        {
            Instance = this;
            // 씬이 변경되어도 파괴되지 않게 하려면 주석 해제
            // DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); // 이미 인스턴스가 있으면 자신을 파괴
        }

        // 초기 상태 설정
        if (settingsPanel != null) settingsPanel.SetActive(false);
        if (stagePanel != null) stagePanel.SetActive(false);
    }

    // 설정 패널 토글 함수
    public void ToggleSettingsPanel()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(!settingsPanel.activeSelf);
            Debug.Log("Settings Panel 활성화 상태: " + settingsPanel.activeSelf);
        }
    }

    // 인벤토리 패널 토글 함수
    public void ToggleStagePanel()
    {
        if (stagePanel != null)
        {
            stagePanel.SetActive(!stagePanel.activeSelf);
            Debug.Log("Inventory Panel 활성화 상태: " + stagePanel.activeSelf);
        }
    }
}
