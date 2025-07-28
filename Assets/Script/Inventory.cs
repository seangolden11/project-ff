using UnityEngine;
using System.Collections.Generic;


public class Inventory : MonoBehaviour
{
    public int inventorySlotLimit = 20; // 인벤토리 슬롯 제한
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public int currentCount;

    [System.Serializable]
    public class InventorySlot
    {
        public Item item;
        public Transform visualItemContainer; // 3D 시각화를 위한 부모 오브젝트
        public int slotNum;
        public bool IsEmpty => item == null;
        GameObject visualItem;

        public void UpdateVisuals()
        {

            if (item != null)
            {

                visualItem = PrefabManager.Instance.InstantiatePrefab(item.itemName, Vector3.zero, Quaternion.identity, visualItemContainer);
                // 아이템이 쌓이는 위치 조정 (예: y축으로 쌓기)
                visualItem.transform.localPosition = new Vector3((slotNum / 4) % 4 - 2, (slotNum / 16) * 1f, slotNum % 4 + 1); // 예시: 0.1f씩 Y축으로 쌓음




            }
            else
            {
                if (visualItem)
                    visualItem.SetActive(false);
            }
        }
        public InventorySlot(int num, Transform transform)
        {
            this.slotNum = num;
            this.visualItemContainer = transform;
            this.item = null;
        }


    }

    void Awake()
    {
        for (int i = 0; i < inventorySlotLimit; i++)
        {
            inventorySlots.Add(new InventorySlot(i, this.transform));
        }
        currentCount = 0;
    }

    public bool AddItem(Item itemToAdd)
    {
        if (inventorySlotLimit < itemToAdd.Size + currentCount)
            return false;

        int count = itemToAdd.Size;
        // 기존 스택 가능한 슬롯 탐색
        foreach (var slot in inventorySlots)
        {
            if (slot.IsEmpty)
            {
                slot.item = itemToAdd;
                slot.UpdateVisuals();
                count--;
                currentCount++;
            }
            if (count == 0)
            {
                return true;
            }
        }
        return true;
    }

    public int DeleteItem(Item item, int amount)
    {

        int removedCount = 0;
        
        // 제거할 아이템을 찾아서 비움
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (!inventorySlots[i].IsEmpty && inventorySlots[i].item.itemName == item.itemName)
            {
                inventorySlots[i].item = null; // 아이템 데이터만 제거 (아직 시각화는 업데이트 안함)
                currentCount--;
                removedCount++;

                if (removedCount*item.Size >= amount)
                {
                    break;
                }
            }
        }

        // 빈 슬롯을 채우기 위해 뒤의 아이템들을 앞으로 당김
        int writeIndex = 0; // 아이템을 쓸 위치
        for (int readIndex = 0; readIndex < inventorySlots.Count; readIndex++)
        {
            if (!inventorySlots[readIndex].IsEmpty) // 비어있지 않은 슬롯을 찾으면
            {
                if (writeIndex != readIndex) // 현재 위치가 쓸 위치와 다르면
                {
                    // 아이템 데이터를 앞으로 이동
                    inventorySlots[writeIndex].item = inventorySlots[readIndex].item;
                    inventorySlots[readIndex].item = null; // 원래 자리는 비움
                }
                writeIndex++; // 다음 쓸 위치로 이동
            }
        }

        // 이동된 아이템들과 비워진 뒷부분 슬롯들의 시각화 업데이트
        // 이 루프에서만 Destroy와 Instantiate를 수행합니다.
        for (int i = 0; i < inventorySlotLimit; i++)
        {
            inventorySlots[i].UpdateVisuals();
        }

        Debug.Log($"'{item.itemName}' {removedCount}개가 인벤토리에서 제거되었습니다. 현재 사용 중인 슬롯: {currentCount}/{inventorySlotLimit}");
        return removedCount/item.Size;
    }
    
    public bool HasItem(Item item)
    {
        foreach (var slot in inventorySlots)
        {
            if (!slot.IsEmpty && slot.item.itemName == item.itemName)
            {
                return true;
            }
        }
        return false;
    }

    // 아이템 제거, 특정 슬롯 아이템 가져오기 등 추가 함수 구현
}