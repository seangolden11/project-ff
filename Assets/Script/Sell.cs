using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Sell : MonoBehaviour
{
    private Inventory inventory;
    public int sellLimit;

    public int level;

    public int transformationTime;

    StageData stageData;

    private Dictionary<PublicDataType.ItemType, int> soldItemCounts = new Dictionary<PublicDataType.ItemType, int>();
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventory = other.GetComponentInChildren<Inventory>();
            TryTakeItemsFromPlayerInventory();
            StartCoroutine(ProcessItemTransformation());

        }
        
    }

    private IEnumerator ProcessItemTransformation()
    {
       
                yield return new WaitForSeconds(transformationTime);      
                
    }

    void Start()
    {
        stageData = GameManager.Instance.stageData;
        
    }

    private void TryTakeItemsFromPlayerInventory()
    {
        // 한 번만 아이템을 가져가도록 플래그를 사용하거나, 다른 트리거 조건을 추가할 수 있습니다.
        // 이 예시에서는 Update에서 계속 호출되므로, 실제 게임에서는 한 번만 실행되도록 로직을 추가해야 합니다.
        // 예: E 키를 눌렀을 때만 작동하도록 등.

        int itemsSuccessfullyTaken = 0;
        int removedCount = 0;

        // 인벤토리 슬롯을 역순으로 순회하여 아이템을 가져옵니다. 
        // 이렇게 하면 앞에서부터 아이템을 삭제했을 때 인덱스 문제가 발생하지 않습니다.
        // 또한 아이템을 종류 상관없이 가져가야 하므로, 단순히 슬롯을 순회하며 비어있지 않은 슬롯에서 아이템을 가져갑니다.
        for (int i = 0; i < inventory.inventoryCount && itemsSuccessfullyTaken < sellLimit + level * sellLimit; i++)
        {
            Inventory.InventorySlot slot = inventory.inventorySlots[i];
            if (!slot.IsEmpty)
            {
                Item itemInSlot = slot.item;

                // 아이템의 Size 단위로 가져가야 하므로, 실제 제거할 아이템의 '개수'를 1로 고정하고,
                // DeleteItem 메서드 내부에서 Size를 고려하여 제거되도록 합니다.
                // Inventory.cs의 DeleteItem 로직이 amount * item.Size <= removedCount 를 체크하므로,
                // amount를 1로 넘기면 해당 아이템 하나의 Size만큼 슬롯이 비워지게 됩니다.
                removedCount = inventory.DeleteItem(itemInSlot, (sellLimit + level * sellLimit) - itemsSuccessfullyTaken);
                MoneyManager.Instance.AddMoney(itemInSlot.sellPrice * removedCount);
                

                PublicDataType.ItemType typeOfSoldItem = GetItemTypeFromItem(itemInSlot); // 아래에 구현할 도우미 함수

                if (soldItemCounts.ContainsKey(typeOfSoldItem))
                {
                    soldItemCounts[typeOfSoldItem] += removedCount; // 제거된 실제 개수만큼 더함
                }
                else
                {
                    soldItemCounts.Add(typeOfSoldItem, removedCount);
                }

                if (removedCount > 0)
                {
                    itemsSuccessfullyTaken += removedCount;
                    Debug.Log($"상점이 '{itemInSlot.itemName}' {removedCount}개 (크기 {itemInSlot.Size} 단위)를 가져갔습니다. 남은 필요 수량: {(sellLimit + level * sellLimit) - itemsSuccessfullyTaken}");
                }
            }
        }

        if (itemsSuccessfullyTaken > 0)
        {
            Debug.Log($"상점이 인벤토리에서 총 {itemsSuccessfullyTaken}개의 아이템을 가져갔습니다.");
            // 아이템을 성공적으로 가져갔으면 상호작용이 완료된 것으로 간주하고,
            // 더 이상 아이템을 가져가지 않도록 상점을 비활성화하거나, 플래그를 설정할 수 있습니다.
            // 예: this.enabled = false; 
            CheckWinCondition();
        }
    }

    private PublicDataType.ItemType GetItemTypeFromItem(Item item)
    {
        // **중요: Item 클래스에 StageData.Itemtype을 정의해야 합니다.**
        // 예시: Item.cs 에 public StageData.Itemtype type; 추가
        // return item.type;

        // 임시로 아이템 이름으로 매핑 (정확한 방법은 Item에 enum 필드를 두는 것)
        if (item.itemName.Contains("EggPowder")) return PublicDataType.ItemType.EggPowder;
        if (item.itemName.Contains("Egg")) return PublicDataType.ItemType.Egg;
        if (item.itemName.Contains("Muffin")) return PublicDataType.ItemType.Muffin;
        // 다른 아이템 타입도 여기에 추가
        return PublicDataType.ItemType.Money; // Money는 판매로 얻는 것이므로 기본적으로 포함 안될 수 있음
    }
    
    public void CheckWinCondition()
    {

        bool allGoalsMet = true; // 모든 목표를 달성했는지 확인하는 플래그

        foreach (var goalItem in stageData.goal.goalItems)
        {
            // 목표 아이템의 현재 판매량
            int currentSoldCount = 0;
            if (soldItemCounts.ContainsKey(goalItem.type))
            {
                currentSoldCount = soldItemCounts[goalItem.type];
            }

            Debug.Log($"Checking Goal for {goalItem.type}: Sold {currentSoldCount} / Needed {goalItem.count}");

            if (currentSoldCount < goalItem.count)
            {
                allGoalsMet = false; // 하나라도 목표 수량에 미달하면 false
                break; // 더 이상 확인할 필요 없음
            }
        }

        if (allGoalsMet)
        {
            Debug.Log("모든 목표 달성! 게임 승리!");
            for (int i = 0; i < inventory.inventoryCount; i++)
            {
                inventory.DeleteItem(inventory.inventorySlots[i].item, 999);
            }
            // 승리 시 처리할 로직 호출
            GameManager.Instance.StageClear();
        }
    }
}
