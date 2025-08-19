
using UnityEngine;
using UnityEngine.SceneManagement; // 씬 관리 기능을 사용하기 위해 필요합니다.

public class SceneChanger : MonoBehaviour
{
    // 이 함수는 버튼 클릭 이벤트에 연결될 함수입니다.
    // 다음 씬의 이름을 문자열로 받습니다.

    public StaminaManager sm;
    public void LoadSceneByName(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneByNameForStart(string sceneName)
    {
        if (GameManager.Instance.stageData.stageID == 0 || DataManager.Instance.GetStageData(GameManager.Instance.stageData.stageID - 1).cleared)
        {
            if (DataManager.Instance.GetHeartData() > 0)
            {
                sm.UseStamina();
                SceneManager.LoadScene(sceneName);
            }
                
        }
        
    }

    // 이 함수는 씬의 빌드 인덱스를 사용하여 씬을 로드할 때 사용합니다.
    // 씬 빌드 인덱스를 정수형으로 받습니다.
    public void LoadSceneByIndex(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}