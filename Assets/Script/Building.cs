using UnityEngine;
using System.Collections;
using System;

public class Building : MonoBehaviour
{
    // ## 기존 Building 변수들 ##
    public string playerTag = "Player";
    public Item itemToTake;
    public Item itemToGive;
    float transformationTime = 10f;
    public BuildingInfo buildinginfo;
    private bool playerInsideTrigger = false;
    public int level;
    private Coroutine transformationCoroutine;

    public bool isPrimary;
    public bool isWorking = false;
    int multiple;
    public Animator anim;

    // ## 테두리 그리기를 위해 추가된 변수들 ##
    [Header("Outline Drawing Components")] // 인스펙터에서 보기 좋게 헤더 추가
    [SerializeField] private Transform topBar;
    [SerializeField] private Transform rightBar;
    [SerializeField] private Transform bottomBar;
    [SerializeField] private Transform leftBar;


    // 플레이어가 트리거 영역에 들어왔을 때 호출
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            Debug.Log("플레이어가 빌딩에 접근했습니다.");
            if (playerInsideTrigger == true)
                return;
            
            transformationCoroutine = StartCoroutine(ProcessItemTransformation(other.gameObject));
        }
    }

    public void Init()
    {
        itemToTake = buildinginfo.itemTakes;
        itemToGive = buildinginfo.itemGives;
        GetComponentsInChildren<SpriteRenderer>()[4].sprite = buildinginfo.itemTakes.sprite;
        multiple = DataManager.Instance.GetJobData()[buildinginfo.id + 3].rank;
        transformationTime /= (multiple * 0.25f) + 1;
        
        // << 추가: 시작할 때 테두리 스케일 초기화
        // ResetOutline();
    }

    public void Start()
    {
        Init();
    }

    public void GetItem(Worker worker)
    {
        transformationCoroutine = StartCoroutine(GetItemCoroutine(worker));
    }

    IEnumerator GetItemCoroutine(Worker worker)
    {
        playerInsideTrigger = true;
        isWorking = true; // << 추가: isWorking 상태 반영
        anim.SetBool("isWorking", true);
        int count = 0;
        for (int i = 0; i < level; i++)
        {
            if (worker.itemCount > 0)
            {
                worker.itemCount--;
                count++;
            }
        }
        
        // << 변경: 단순히 기다리는 대신, 테두리를 그리는 코루틴을 실행하고 끝날 때까지 기다림
        yield return StartCoroutine(DrawOutlineRoutine(transformationTime));

        for (int i = 0; i < count; i++)
        {
            PrefabManager.Instance.Get(itemToGive.Name, transform.position, transform.rotation).transform.SetParent(transform);
        }
        playerInsideTrigger = false;
        isWorking = false; // << 추가: isWorking 상태 반영
        anim.SetBool("isWorking", false);
    }

    private IEnumerator ProcessItemTransformation(GameObject player)
    {
        Inventory playerInventory = player.GetComponentInChildren<Inventory>();
        if (playerInventory != null && playerInventory.HasItem(itemToTake))
        {
            int count = playerInventory.DeleteItem(itemToTake, level);
            playerInsideTrigger = true;
            isWorking = true; // << 추가: isWorking 상태 반영
            anim.SetBool("isWorking", true);

            Debug.Log($"{transformationTime}초 후에 아이템이 변환됩니다...");

            // << 변경: 단순히 기다리는 대신, 테두리를 그리는 코루틴을 실행하고 끝날 때까지 기다림
            yield return StartCoroutine(DrawOutlineRoutine(transformationTime));

            Debug.Log($"{itemToTake}이(가) {itemToGive}으로 변환되었습니다.");
            for (int i = 0; i < count; i++)
            {
                PrefabManager.Instance.Get(itemToGive.Name, transform.position, transform.rotation).transform.SetParent(transform);
            }
            playerInsideTrigger = false;
            isWorking = false; // << 추가: isWorking 상태 반영
            anim.SetBool("isWorking", false);
        }
        else
        {
            if (playerInventory == null) Debug.LogError("플레이어에 PlayerInventory 스크립트가 없습니다!");
            else Debug.Log($"인벤토리에 {itemToTake} 아이템이 없습니다.");
        }
    }

    // ## 테두리 그리기를 위해 추가된 코루틴과 메소드 ##

    /// <summary>
    /// 지정된 시간 동안 테두리를 시계 방향으로 그리는 코루틴
    /// </summary>
    private IEnumerator DrawOutlineRoutine(float duration)
    {
        ResetOutline(); // 그리기를 시작하기 전 항상 초기화

        float timer = 0f;
        float timePerSide = duration / 4.0f;
        float multiplier = 10f;

        // 위쪽 막대 그리기
        while (timer < timePerSide)
        {
            topBar.localScale = new Vector3((timer / timePerSide) * multiplier, topBar.localScale.y, topBar.localScale.z);
            timer += Time.deltaTime;
            yield return null;
        }
        topBar.localScale = new Vector3(multiplier, topBar.localScale.y, topBar.localScale.z);
        timer = 0f;

        // 오른쪽 막대 그리기
        while (timer < timePerSide)
        {
            rightBar.localScale = new Vector3(rightBar.localScale.x, (timer / timePerSide)*multiplier, rightBar.localScale.z);
            timer += Time.deltaTime;
            yield return null;
        }
        rightBar.localScale = new Vector3(rightBar.localScale.x, multiplier, rightBar.localScale.z);
        timer = 0f;
        
        // 아래쪽 막대 그리기
        while (timer < timePerSide)
        {
            bottomBar.localScale = new Vector3((timer / timePerSide)*multiplier, bottomBar.localScale.y, bottomBar.localScale.z);
            timer += Time.deltaTime;
            yield return null;
        }
        bottomBar.localScale = new Vector3(multiplier, bottomBar.localScale.y, bottomBar.localScale.z);
        timer = 0f;

        // 왼쪽 막대 그리기
        while (timer < timePerSide)
        {
            leftBar.localScale = new Vector3(leftBar.localScale.x, (timer / timePerSide)*multiplier, leftBar.localScale.z);
            timer += Time.deltaTime;
            yield return null;
        }
        leftBar.localScale = new Vector3(leftBar.localScale.x, multiplier, leftBar.localScale.z);
    }

    /// <summary>
    /// 모든 테두리 막대의 스케일을 0으로 되돌리는 함수
    /// </summary>
    private void ResetOutline()
    {
        topBar.localScale = new Vector3(0, topBar.localScale.y, topBar.localScale.z);
        rightBar.localScale = new Vector3(rightBar.localScale.x, 0, rightBar.localScale.z);
        bottomBar.localScale = new Vector3(0, bottomBar.localScale.y, bottomBar.localScale.z);
        leftBar.localScale = new Vector3(leftBar.localScale.x, 0, leftBar.localScale.z);
    }
}