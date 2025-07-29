using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    // 정적 인스턴스를 통해 어디서든 MoneyManager에 접근할 수 있도록 합니다.
    public static MoneyManager Instance { get; private set; }

    [SerializeField]
    private int currentMoney = 0; // 플레이어의 현재 돈, 인스펙터에서 확인 가능하도록 SerializeField 사용

    // 돈이 변경될 때 알림을 받을 수 있도록 이벤트 추가 (UI 업데이트 등에 활용)
    public delegate void OnMoneyChanged(int newMoney);
    public event OnMoneyChanged onMoneyChangedCallback;

    void Awake()
    {
        // 싱글톤 패턴 구현: 중복 생성을 막고, 어디서든 접근 가능하게 합니다.
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않도록 설정
        }
    }

    // 현재 돈을 가져오는 메서드
    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    // 돈을 추가하는 메서드
    public void AddMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("음수 금액을 추가할 수 없습니다. AddMoney 대신 RemoveMoney를 사용하세요.");
            return;
        }
        currentMoney += amount;
        Debug.Log($"돈 추가: {amount}. 현재 돈: {currentMoney}");
        onMoneyChangedCallback?.Invoke(currentMoney); // 돈 변경 이벤트 호출
    }

    // 돈을 감소시키는 메서드
    public bool TryRemoveMoney(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("음수 금액을 제거할 수 없습니다.");
            return false;
        }

        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            Debug.Log($"돈 제거: {amount}. 현재 돈: {currentMoney}");
            onMoneyChangedCallback?.Invoke(currentMoney); // 돈 변경 이벤트 호출
            return true;
        }
        else
        {
            Debug.Log("돈이 부족합니다.");
            return false;
        }
    }
}