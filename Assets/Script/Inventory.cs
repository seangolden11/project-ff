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
                visualItem.transform.localPosition = new Vector3((slotNum / 4) % 4 - 2, (slotNum/16) * 1f,slotNum % 4 + 1); // 예시: 0.1f씩 Y축으로 쌓음
                
                
                

            }
            else
            {
                visualItem.SetActive(false);
            }
        }
        public InventorySlot(int num, Transform transform)
        {
            this.slotNum = num;
            this.visualItemContainer = transform;
        }


    }

    void Awake()
    {
        for (int i = 0; i < inventorySlotLimit; i++)
        {
            inventorySlots.Add(new InventorySlot(i,this.transform));
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

    public void DeleteItem(Item item, int amount) {
        
    }

    // 아이템 제거, 특정 슬롯 아이템 가져오기 등 추가 함수 구현
}