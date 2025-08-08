using UnityEngine;
using TMPro;
using System; // TextMeshPro를 사용하기 위해 필요합니다.

public class StarText : MonoBehaviour
{
    public TextMeshProUGUI Text; // Unity 에디터에서 연결할 TextMeshProUGUI 컴포넌트

    public int isStar;

    void Start()
    {
        switch (isStar)
        {
            case 0:
                if (Text != null)
                {
                    Text.text = $"X {DataManager.Instance.GetStarData()}"; // 텍스트 내용을 업데이트합니다.
                }
                break;
            case 1:
                if (Text != null)
                {
                    Text.text = $"X {DataManager.Instance.GetHeartData()}"; // 텍스트 내용을 업데이트합니다.
                }
                break;
            default:
                break;
    }
    }

}