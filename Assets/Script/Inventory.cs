using UnityEngine;
using System.Collections.Generic;
using System;
using Unity.Mathematics;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;


public class Inventory : MonoBehaviour
{
    public int inventorySlotLimit = 30; // 인벤토리 슬롯 제한
    public List<InventorySlot> inventorySlots = new List<InventorySlot>();

    public List<Item> items = new List<Item>();

    public int inventoryCount = 0;

    public GameObject prefab;
    public Transform tf;

    bool isMoving = false;

    public List<GameObject> obj = new List<GameObject>();

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

    // public void UpdateAddVisuals(Item item, Transform target)
    // {

    //     int index = 0;
    //     foreach (var slot in inventorySlots)
    //     {
    //         if (slot.item == item)
    //             break;
    //         index += slot.quantity * slot.item.Size;
    //     }

    //     for (int i = 0; i < item.Size; i++)
    //     {

    //         GameObject newobject1 = PrefabManager.Instance.Get(item.itemName, target.position, Quaternion.identity);
    //         newobject1.GetComponent<MoveAndDisappear>().StartMove(transform);
    //         newobject1.GetComponent<MoveAndDisappear>().enabled = true;

    //     }

    //     StartCoroutine(OnLastItemDestroyedCoroutine(index, item));

    // }

    // private IEnumerator OnLastItemDestroyedCoroutine(int index, Item item)
    // {
    //     yield return new WaitForSeconds(0.6f);

    //     for (int i = 0; i < item.Size; i++)
    //     {
    //         GameObject newobject = PrefabManager.Instance.Get(item.itemName, Vector3.zero, Quaternion.identity);
    //         newobject.transform.parent = this.transform;
    //         newobject.transform.localRotation = quaternion.identity;
    //         visualItem.Insert(index, newobject);
    //     }

    //     if (!isMoving)
    //     {
    //         isMoving = true;

    //         for (int j = 0; j < visualItem.Count; j++)
    //         {
    //             visualItem[j].transform.localPosition = GetLocalPositionForVisual(j);
    //         }

    //         isMoving = false;
    //     }

    //     InitCanvas();
    // }

    // //시간이 남으면 최적화
    // public void UpdateDeleteVisuals(Item item, int amount, Transform target)
    // {
    //     int index = 0;
    //     foreach (var slot in inventorySlots)
    //     {
    //         if (slot.item == item)
    //             break;
    //         index += slot.quantity * slot.item.Size;
    //     }
    //     for (int i = 0; i < amount * item.Size; i++)
    //     {
    //         Debug.Log(index);
    //         GameObject newobject = visualItem[index];
    //         newobject.transform.parent = null;
    //         newobject.GetComponent<MoveAndDisappear>().StartMove(target);
    //         newobject.GetComponent<MoveAndDisappear>().enabled = true;

    //         visualItem.Remove(newobject);
    //     }

    //     for (int i = 0; i < visualItem.Count; i++)
    //     {
    //         visualItem[i].transform.localPosition = GetLocalPositionForVisual(i);
    //     }

    //     InitCanvas();
    // }
    
    public int DeleteItem(Item item, int amount, Transform target)
{
    InventorySlot slot = inventorySlots[item.inventoryNum];
    // 제거할 아이템이 있는지 확인은 유지
    if (slot.quantity <= 0) return 0;

    int removedCount = Mathf.Min(amount, slot.quantity);

    // 데이터 변경 로직 삭제!
    //  slot.quantity -= removedCount;
     inventoryCount -= item.Size * removedCount;
    
    // 시각적 처리는 큐에 작업으로 추가 (기존과 동일)
    actionQueue.Enqueue(new InventoryAction(ActionType.Remove, item, removedCount, target));
    
    
    // Debug.Log는 실제 처리 후로 옮기는 것이 더 정확함
    return removedCount;
}

    private IEnumerator HandleActionCoroutine(InventoryAction action)
    {
        try
        {

            InventorySlot slot1 = inventorySlots[action.Item.inventoryNum];
            if (action.Type == ActionType.Add)
            {
                // inventoryCount += action.Item.Size * action.Amount;
                slot1.quantity += action.Amount;
                for (int i = 0; i < action.Amount * action.Item.Size; i++)
            {
                Transform from = (action.Type == ActionType.Add) ? action.Target : this.transform;
                Transform to = (action.Type == ActionType.Add) ? this.transform : action.Target;

                GameObject newobject1 = PrefabManager.Instance.Get(action.Item.itemName, from.position, Quaternion.identity);
                newobject1.GetComponent<MoveAndDisappear>().StartMove(to);
                
            }
            }
            else // ActionType.Remove
            {
                // inventoryCount -= action.Item.Size * action.Amount;
                slot1.quantity -= action.Amount;
                Debug.Log($"'{action.Item.itemName}' {action.Amount}개가 인벤토리에서 제거되었습니다.");
            }
           

            // 2. 애니메이션 시간만큼 대기
            if (action.Type == ActionType.Add)
            {
                
                yield return new WaitForSeconds(0.6f);
                
            }
            else
            {
                yield return new WaitForSeconds(0);
            }

            // 3. 인덱스 계산 (이 시점에는 다른 작업이 끼어들지 않아 안전함)
            int index = 0;
            foreach (var slot in inventorySlots)
            {
                if (slot.item == action.Item)
                    break;
                index += slot.quantity * slot.item.Size;
            }

            // 4. 작업 타입에 따라 리스트 수정
            if (action.Type == ActionType.Add)
            {
                // 추가 로직
                for (int i = 0; i < action.Item.Size; i++)
                {
                    GameObject newobject = PrefabManager.Instance.Get(action.Item.itemName, Vector3.zero, Quaternion.identity);
                    newobject.transform.parent = this.transform;
                    newobject.transform.localRotation = quaternion.identity;
                    Debug.Log(index);
                    visualItem.Insert(index, newobject);
                }
            }
            else // ActionType.Remove
            {
                // 삭제 로직
                for (int i = 0; i < action.Amount * action.Item.Size; i++)
                {
                    // index 위치에서 계속 제거 (리스트가 줄어드므로 항상 같은 index)
                    GameObject objectToRemove = visualItem[index];
                    objectToRemove.GetComponent<MoveAndDisappear>().StartMove(action.Target);
                    visualItem.RemoveAt(index);
                }
            }

            // 5. 전체 위치 재정렬
            for (int j = 0; j < visualItem.Count; j++)
            {
                visualItem[j].transform.localPosition = GetLocalPositionForVisual(j);
            }

            // 6. UI 캔버스 업데이트
            InitCanvas();
        }
        finally
        {
            // 모든 처리가 끝나면 잠금을 해제하여 다음 작업이 시작될 수 있도록 함
            isProcessingQueue = false;
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

public bool AddItem(Item itemToAdd, Transform target)
{
    // 예상 공간 확인은 유지할 수 있으나, 실제 데이터 변경은 하지 않음
    if (inventorySlotLimit < itemToAdd.Size + inventoryCount)
        return false;

    // 데이터 변경 로직 삭제!
     InventorySlot slot = inventorySlots[itemToAdd.inventoryNum];
     inventoryCount += itemToAdd.Size;
    // slot.quantity++;

    // 시각적 처리는 큐에 작업으로 추가 (기존과 동일)
    actionQueue.Enqueue(new InventoryAction(ActionType.Add, itemToAdd, 1, target));
    

    return true;
}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PickableItem"))
        {
            AddItem(other.GetComponent<ItemPickup>().item, other.transform);
            PrefabManager.Instance.Release(other.gameObject);
        }
    }

    // public int DeleteItem(Item item, int amount, Transform target)
    // {

    //     int removedCount = 0;

    //     // 제거할 아이템을 찾아서 비움
    //     InventorySlot slot = inventorySlots[item.inventoryNum];
    //     if (slot.quantity > amount)
    //     {
    //         slot.quantity -= amount;
    //         removedCount = amount;
    //     }
    //     else
    //     {
    //         removedCount = slot.quantity;
    //         slot.quantity = 0;
    //     }

    //     inventoryCount -= item.Size * removedCount;
    //     UpdateDeleteVisuals(item, removedCount, target);


    //     Debug.Log($"'{item.itemName}' {removedCount}개가 인벤토리에서 제거되었습니다. 현재 사용 중인 슬롯: {inventoryCount}/{inventorySlotLimit}");
    //     return removedCount;
    // }

    public bool HasItem(Item item)
    {

        return !inventorySlots[item.inventoryNum].IsEmpty;
    }

    public void InitCanvas()
    {

        foreach (GameObject temp in obj)
        {
            Destroy(temp);
        }
        obj.Clear();

        foreach (InventorySlot slot in inventorySlots)
        {
            if (!slot.IsEmpty)
            {
                GameObject tempobj = Instantiate(prefab, tf);
                tempobj.GetComponentsInChildren<Image>()[0].sprite = slot.item.sprite;
                tempobj.GetComponentInChildren<TextMeshProUGUI>().text = $"{slot.quantity}";
                obj.Add(tempobj);
            }
        }
    }

    public enum ActionType { Add, Remove }

    public class InventoryAction
    {
        public ActionType Type;
        public Item Item;
        public int Amount;
        public Transform Target; // 애니메이션을 위한 목표 Transform

        public InventoryAction(ActionType type, Item item, int amount, Transform target)
        {
            Type = type;
            Item = item;
            Amount = amount;
            Target = target;
        }
    }
    private Queue<InventoryAction> actionQueue = new Queue<InventoryAction>();
    private bool isProcessingQueue = false; // 현재 작업 처리 중인지 확인하는 플래그

    private IEnumerator ProcessActionQueue()
    {
        // 이 오브젝트가 살아있는 동안 계속 실행
        while (true)
        {
            // 처리 중이 아니고, 큐에 작업이 있으면
            if (!isProcessingQueue && actionQueue.Count > 0)
            {
                isProcessingQueue = true; // 처리 시작! (잠금)
                InventoryAction action = actionQueue.Dequeue(); // 대기열에서 가장 오래된 작업 꺼내기
                yield return StartCoroutine(HandleActionCoroutine(action)); // 작업 처리 코루틴 실행 및 종료 대기
            }
            else
            {
                yield return null; // 할 일 없으면 한 프레임 대기
            }
        }
    }

    void Start()
    {
        StartCoroutine(ProcessActionQueue());
    }



    // 아이템 제거, 특정 슬롯 아이템 가져오기 등 추가 함수 구현
}