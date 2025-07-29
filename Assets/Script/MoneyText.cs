using UnityEngine;
using TMPro; // TextMeshPro를 사용하기 위해 필요합니다.

public class MoneyText : MonoBehaviour
{
    public TextMeshProUGUI moneyText; // Unity 에디터에서 연결할 TextMeshProUGUI 컴포넌트

    void Start()
    {
        // MoneyManager 인스턴스가 존재하는지 확인
        if (MoneyManager.Instance == null)
        {
            Debug.LogError("MoneyManager.Instance가 씬에 존재하지 않습니다. MoneyManager 스크립트가 적용된 GameObject가 있는지 확인하세요.");
            return;
        }

        // MoneyManager의 돈 변경 이벤트에 현재 스크립트의 메서드를 등록합니다.
        // 이렇게 하면 MoneyManager에서 돈이 변경될 때마다 OnMoneyChanged가 호출됩니다.
        MoneyManager.Instance.onMoneyChangedCallback += OnMoneyChanged;

        // 게임 시작 시 현재 돈으로 UI를 초기화합니다.
        UpdateMoneyDisplay(MoneyManager.Instance.GetCurrentMoney());
    }

    void OnDestroy()
    {
        // 스크립트가 파괴될 때 이벤트 등록을 해제하여 메모리 누수를 방지합니다.
        if (MoneyManager.Instance != null)
        {
            MoneyManager.Instance.onMoneyChangedCallback -= OnMoneyChanged;
        }
    }

    // MoneyManager에서 돈이 변경될 때 호출될 메서드
    void OnMoneyChanged(int newMoney)
    {
        UpdateMoneyDisplay(newMoney);
    }

    // 실제 UI 텍스트를 업데이트하는 메서드
    void UpdateMoneyDisplay(int money)
    {
        if (moneyText != null)
        {
            moneyText.text = $"{money}$"; // 텍스트 내용을 업데이트합니다.
        }
    }
}