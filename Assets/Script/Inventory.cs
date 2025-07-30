using UnityEngine;
using System.Collections.Generic;
using System;


public class Inventory : MonoBehaviour
{
    public int inventorySlotLimit = 20; // 인벤토리 슬롯 제한
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public List<Item> items = new List<Item>();

    public int inventoryCount =0;

    public List<GameObject> visualItem = new List<GameObject>();

    [System.Serializable]
    public class InventorySlot
    {
        public Item item;
        public Transform visualItemContainer; // 3D 시각화를 위한 부모 오브젝트
        public int slotNum;

        public int quantity;
        public bool IsEmpty => quantity == 0;
        

        
        public InventorySlot(int num, Transform transform, Item item)
        {
            this.slotNum = num;
            this.visualItemContainer = transform;
            this.item = item;
            this.quantity = 0;
            
        }


    }

    public void UpdateAddVisuals(Item item)
    {
        int i = inventoryCount - item.Size;

        for (; i < inventoryCount; i++)
        {
            GameObject newobject = PrefabManager.Instance.Get(item.itemName, Vector3.zero, Quaternion.identity);
            newobject.transform.parent = this.transform;
            // 아이템이 쌓이는 위치 조정 (예: y축으로 쌓기)
            newobject.transform.localPosition = new Vector3((i / 4) % 4 - 1.5f, (i / 16) * 1f, i % 4 + 1); // 예시: 0.1f씩 Y축으로 쌓음
            visualItem.Add(newobject);
        }
    }
    //시간이 남으면 최적화
    public void UpdateDeleteVisuals(Item item = null, int amount = 0)
    {
        for (int i = 0; i < inventoryCount; i++)
        {
            GameObject newobject = visualItem[i];
            PrefabManager.Instance.Release(newobject);
            visualItem.Remove(newobject);
        }

            for (int i=0;i<inventorySlots.Count;i++)
            {

                for (int j = 0; j < inventorySlots[i].quantity; j++)
                {

                    UpdateAddVisuals(inventorySlots[i].item);
                }
                




            }
    }

    void Awake()
    {

        for (int i = 0; i < items.Count; i++)
        {
            inventorySlots.Add(new InventorySlot(i,this.transform,items[i]));
        }
        
    }

    public bool AddItem(Item itemToAdd)// 빈 슬롯 찬슬롯 나눠서 관리하기
    {
        if (inventorySlotLimit < itemToAdd.Size + inventoryCount)
            return false;


        InventorySlot slot = inventorySlots[itemToAdd.inventoryNum];
        inventoryCount += itemToAdd.Size;
        slot.quantity++;
        
        UpdateAddVisuals(itemToAdd);
        
               
        
        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickableItem"))
        {
            AddItem(other.GetComponent<ItemPickup>().item);
            PrefabManager.Instance.Release(other.gameObject);
        }
    }

    public int DeleteItem(Item item, int amount)
    {

        int removedCount = 0;

        // 제거할 아이템을 찾아서 비움
        InventorySlot slot = inventorySlots[item.inventoryNum];
        if (slot.quantity > amount)
        {
            slot.quantity -= amount;
            removedCount = amount;
        }
        else
        {
            removedCount = slot.quantity;
            slot.quantity = 0;
        }

        
        UpdateDeleteVisuals(item, amount);
        

        Debug.Log($"'{item.itemName}' {removedCount}개가 인벤토리에서 제거되었습니다. 현재 사용 중인 슬롯: {inventoryCount}/{inventorySlotLimit}");
        return removedCount;
    }
    
    public bool HasItem(Item item)
    {
        
        return inventorySlots[item.inventoryNum].IsEmpty;;
    }

    // 아이템 제거, 특정 슬롯 아이템 가져오기 등 추가 함수 구현
}