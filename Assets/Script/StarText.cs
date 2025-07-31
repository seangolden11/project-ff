using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 필요합니다.

public class StarText : MonoBehaviour
{
    public TextMeshProUGUI starText; // Unity 에디터에서 연결할 TextMeshProUGUI 컴포넌트

    void Start()
    {

        if (starText != null)
        {
            starText.text = $"X {PlayerPrefs.GetInt("Star")}"; // 텍스트 내용을 업데이트합니다.
        }
    }

}