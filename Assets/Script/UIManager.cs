using UnityEngine;

public class UIManager : MonoBehaviour
{
    // 싱글톤 인스턴스
    public static UIManager Instance { get; private set; }

    [SerializeField] // 인스펙터에서 할당할 수 있도록 SerializeField 사용
    private GameObject settingsPanel;
    [SerializeField]
    private GameObject stagePanel;
    [SerializeField]
    private GameObject upgradePanel;

    public GameObject joystickObject;

    public StarText st;

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
        if (upgradePanel != null) upgradePanel.SetActive(false);

        

    
        // joystickObject가 할당되었는지 확인
        if (joystickObject != null)
        {
        // 전처리기 지시문 시작: iOS 또는 Android 또는 유니티 에디터일 때만 아래 코드를 포함
#if UNITY_IOS || UNITY_ANDROID
        // 모바일 환경이거나 에디터에서 테스트 중일 때는 조이스틱을 활성화
        joystickObject.SetActive(true);
        Debug.Log("모바일 플랫폼 또는 에디터이므로 조이스틱을 활성화합니다.");
#else
        // 그 외의 모든 플랫폼(PC, Mac 등)에서는 조이스틱을 비활성화
        joystickObject.SetActive(false);
        Debug.Log("PC 플랫폼이므로 조이스틱을 비활성화합니다.");
#endif
// 전처리기 지시문 끝
        }


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
            Debug.Log("Stage Panel 활성화 상태: " + stagePanel.activeSelf);
        }
    }
    
    public void ToggleUpgradePanel()
    {
        if (upgradePanel != null)
        {
            upgradePanel.SetActive(!upgradePanel.activeSelf);
            st.Init();

            Debug.Log("Upgrade Panel 활성화 상태: " + upgradePanel.activeSelf);
        }
    }
}
