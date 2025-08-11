using UnityEngine;
using System.Collections;

public class Building : MonoBehaviour
{ public string playerTag = "Player"; // 플레이어 GameObject의 태그 (유니티에서 설정)
    public Item itemToTake; // 인벤토리에서 가져올 아이템 이름
    public Item itemToGive; // 변환 후 줄 아이템 이름
    public float transformationTime = 1f; // 아이템 변환에 걸리는 시간 (초)
    public BuildingInfo buildinginfo;
    private bool playerInsideTrigger = false;
    public int level;
    private Coroutine transformationCoroutine; // 코루틴 참조를 저장하여 중지할 수 있도록

    public Animator anim;

    // 플레이어가 트리거 영역에 들어왔을 때 호출
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("플레이어가 빌딩에 접근했습니다.");
            if (playerInsideTrigger == true)
                return;

            // 아이템 변환 프로세스 시작 (코루틴)
            transformationCoroutine = StartCoroutine(ProcessItemTransformation(other.gameObject));
        }
    }

    public void Init()
    {
        itemToTake = buildinginfo.itemTakes;
        itemToGive = buildinginfo.itemGives;
        
    }

    public void Start()
    {
        Init();
    }



    // 아이템 변환 프로세스를 처리하는 코루틴
    private IEnumerator ProcessItemTransformation(GameObject player)
    {
        // 1. 플레이어 인벤토리에서 아이템 가져오기
        // 이 부분은 플레이어의 인벤토리 시스템에 따라 달라집니다.
        // 예시: PlayerInventory 스크립트에 TakeItem 메서드가 있다고 가정
        Inventory playerInventory = player.GetComponentInChildren<Inventory>();
        if (playerInventory != null)
        {
            if (playerInventory.HasItem(itemToTake))
            {
                int count;
                Debug.Log($"{itemToTake} 아이템을 인벤토리에서 가져옵니다.");
                count = playerInventory.DeleteItem(itemToTake, level);
                playerInsideTrigger = true;
                anim.SetBool("isWorking", true);

                // 2. 10초 대기
                Debug.Log($"{transformationTime}초 후에 아이템이 변환됩니다...");
                yield return new WaitForSeconds(transformationTime);


                // 3. 다른 아이템으로 변환하여 인벤토리에 추가
                Debug.Log($"{itemToTake}이(가) {itemToGive}으로 변환되었습니다. 인벤토리에 추가합니다.");
                for (int i = 0; i < count; i++)
                {
                    PrefabManager.Instance.Get(itemToGive.Name, transform.position, transform.rotation);
                }
                playerInsideTrigger = false;
                anim.SetBool("isWorking", false);
                
                
            }
            else
            {
                Debug.Log($"인벤토리에 {itemToTake} 아이템이 없습니다.");
            }
        }
        else
        {
            Debug.LogError("플레이어에 PlayerInventory 스크립트가 없습니다!");
        }
    }
}
