using UnityEngine;

public class Sell : MonoBehaviour
{
    private Inventory inventory;
    public int sellLimit;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inventory = other.GetComponentInChildren<Inventory>();
            TryTakeItemsFromPlayerInventory();

        }
    }
    
     private void TryTakeItemsFromPlayerInventory()
    {
        // 한 번만 아이템을 가져가도록 플래그를 사용하거나, 다른 트리거 조건을 추가할 수 있습니다.
        // 이 예시에서는 Update에서 계속 호출되므로, 실제 게임에서는 한 번만 실행되도록 로직을 추가해야 합니다.
        // 예: E 키를 눌렀을 때만 작동하도록 등.

        int itemsSuccessfullyTaken = 0;
        
        // 인벤토리 슬롯을 역순으로 순회하여 아이템을 가져옵니다. 
        // 이렇게 하면 앞에서부터 아이템을 삭제했을 때 인덱스 문제가 발생하지 않습니다.
        // 또한 아이템을 종류 상관없이 가져가야 하므로, 단순히 슬롯을 순회하며 비어있지 않은 슬롯에서 아이템을 가져갑니다.
        for (int i = inventory.inventorySlots.Count - 1; i >= 0 && itemsSuccessfullyTaken < sellLimit; i--)
        {
            Inventory.InventorySlot slot = inventory.inventorySlots[i];
            if (!slot.IsEmpty)
            {
                Item itemInSlot = slot.item;
                
                // 아이템의 Size 단위로 가져가야 하므로, 실제 제거할 아이템의 '개수'를 1로 고정하고,
                // DeleteItem 메서드 내부에서 Size를 고려하여 제거되도록 합니다.
                // Inventory.cs의 DeleteItem 로직이 amount * item.Size <= removedCount 를 체크하므로,
                // amount를 1로 넘기면 해당 아이템 하나의 Size만큼 슬롯이 비워지게 됩니다.
                int removedCount = inventory.DeleteItem(itemInSlot, 1);
                MoneyManager.Instance.AddMoney(itemInSlot.sellPrice);
                
                if (removedCount > 0)
                {
                    itemsSuccessfullyTaken += removedCount;
                    Debug.Log($"상점이 '{itemInSlot.itemName}' {removedCount}개 (크기 {itemInSlot.Size} 단위)를 가져갔습니다. 남은 필요 수량: {sellLimit - itemsSuccessfullyTaken}");
                }
            }
        }

        if (itemsSuccessfullyTaken > 0)
        {
            Debug.Log($"상점이 인벤토리에서 총 {itemsSuccessfullyTaken}개의 아이템을 가져갔습니다.");
            // 아이템을 성공적으로 가져갔으면 상호작용이 완료된 것으로 간주하고,
            // 더 이상 아이템을 가져가지 않도록 상점을 비활성화하거나, 플래그를 설정할 수 있습니다.
            // 예: this.enabled = false; 
        }
    }
}
