using UnityEngine;
using TMPro;
using System; // TextMeshPro를 사용하기 위해 필요합니다.

public class StarText : MonoBehaviour
{
    public TextMeshProUGUI Text; // Unity 에디터에서 연결할 TextMeshProUGUI 컴포넌트

    void Start()
    {   
        Init(); // 텍스트 내용을 업데이트합니다.
        
    }

    public void Init()
    {
        if (Text != null)
        {
            Text.text = $"X {DataManager.Instance.GetStarData()}";
        }
    }

}