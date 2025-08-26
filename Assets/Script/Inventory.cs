using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.Mathematics;


public class Inventory : MonoBehaviour
{
    public int inventorySlotLimit = 30; // 인벤토리 슬롯 제한
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public List<Item> items = new List<Item>();

    public int inventoryCount = 0;

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
    private Vector3 GetLocalPositionForVisual(int index)
    {
        // 예시: X축으로 4개 아이템, Z축으로 4개 줄, Y축으로 16개 아이템마다 한 층
        float xPos = (index % 4) * 0.5f - 0.75f; // 필요에 따라 -1.5f를 조정하여 중앙에 배치
        float yPos = (index / 16) * 0.5f;
        float zPos = ((index / 4) % 4) * -0.5f - 1f; // 필요에 따라 +1f를 조정하여 깊이 조절
        return new Vector3(xPos, yPos, zPos);
    }

    public void UpdateAddVisuals(Item item,Transform target)
    {

        int index = 0;
        foreach (var slot in inventorySlots)
        {
            if (slot.item == item)
                break;
            index += slot.quantity * slot.item.Size;
        }

        for (int i = 0; i < item.Size; i++)
        {
            GameObject newobject = PrefabManager.Instance.Get(item.itemName, Vector3.zero, Quaternion.identity);
            GameObject newobject1 = PrefabManager.Instance.Get(item.itemName, target.position, Quaternion.identity);
            newobject1.GetComponent<MoveAndDisappear>().StartMove(transform);
            newobject1.GetComponent<MoveAndDisappear>().enabled = true;
            newobject.transform.parent = this.transform;
            newobject.transform.localRotation = quaternion.identity;
            visualItem.Insert(index, newobject);
        }
        for (int j = 0; j < visualItem.Count; j++)
        {
            visualItem[j].transform.localPosition = GetLocalPositionForVisual(j);
        }
    }
    //시간이 남으면 최적화
    public void UpdateDeleteVisuals(Item item, int amount,Transform target)
    {
        int index = 0;
        foreach (var slot in inventorySlots)
        {
            if (slot.item == item)
                break;
            index += slot.quantity * slot.item.Size;
        }
        for (int i = 0; i < amount * item.Size; i++)
        {
            Debug.Log(index);
            GameObject newobject = visualItem[index];
            newobject.transform.parent = null;
            newobject.GetComponent<MoveAndDisappear>().StartMove(target);
            newobject.GetComponent<MoveAndDisappear>().enabled = true;
            
            visualItem.Remove(newobject);
        }

        for (int i = 0; i < visualItem.Count; i++)
        {
            visualItem[i].transform.localPosition = GetLocalPositionForVisual(i);
        }
    }

    void Awake()
    {

        for (int i = 0; i < items.Count; i++)
        {
            inventorySlots.Add(new InventorySlot(i, this.transform, items[i]));
        }
        inventorySlotLimit += inventorySlotLimit * DataManager.Instance.GetUpgradeData(9).level;

    }

    public bool AddItem(Item itemToAdd, Transform target)// 빈 슬롯 찬슬롯 나눠서 관리하기
    {
        if (inventorySlotLimit < itemToAdd.Size + inventoryCount)
            return false;


        InventorySlot slot = inventorySlots[itemToAdd.inventoryNum];
        inventoryCount += itemToAdd.Size;
        slot.quantity++;

        UpdateAddVisuals(itemToAdd,target);



        return true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickableItem"))
        {
            AddItem(other.GetComponent<ItemPickup>().item,other.transform);
            PrefabManager.Instance.Release(other.gameObject);
        }
    }

    public int DeleteItem(Item item, int amount, Transform target)
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

        inventoryCount -= item.Size * removedCount;
        UpdateDeleteVisuals(item, removedCount,target);


        Debug.Log($"'{item.itemName}' {removedCount}개가 인벤토리에서 제거되었습니다. 현재 사용 중인 슬롯: {inventoryCount}/{inventorySlotLimit}");
        return removedCount;
    }

    public bool HasItem(Item item)
    {

        return !inventorySlots[item.inventoryNum].IsEmpty;
    }
    
    

    // 아이템 제거, 특정 슬롯 아이템 가져오기 등 추가 함수 구현
}