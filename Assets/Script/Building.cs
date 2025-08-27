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

    // ## 이동 오브젝트 설정을 위한 변수들 ##
    [Header("Progress Settings")]
    public Transform movingObject; // 작업 중 (0,0,0)으로 이동할 오브젝트의 Transform
    public GameObject progressBarObject; // 진행도 관련 오브젝트들을 감싸는 부모 (보이기/숨기기용)
    private Vector3 initialObjectPosition; // 오브젝트의 초기 위치 저장

    public SpriteRenderer givesp;
    public SpriteRenderer takesp;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag) && !isWorking)
        {
            if (playerInsideTrigger == true) return;
            transformationCoroutine = StartCoroutine(ProcessItemTransformation(other.gameObject));
        }
    }

    public void Init()
    {
        itemToTake = buildinginfo.itemTakes;
        itemToGive = buildinginfo.itemGives;
        multiple = DataManager.Instance.GetJobData()[buildinginfo.id + 3].rank;
        transformationTime /= (multiple * 0.25f) + 1;
        SpriteRenderer[] sr = GetComponentsInChildren<SpriteRenderer>();

        givesp.sprite = itemToGive.sprite;
        takesp.sprite = itemToTake.sprite;

        // 이동할 오브젝트의 초기 위치 저장 및 숨기기
        if (movingObject != null)
        {
            initialObjectPosition = movingObject.localPosition;
        }
        ResetOutline();
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
        isWorking = true;
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
        
        yield return StartCoroutine(DrawOutlineRoutine(transformationTime));

        for (int i = 0; i < count; i++)
        {
            PrefabManager.Instance.Get(itemToGive.Name, transform.position, transform.rotation).transform.SetParent(transform);
        }
        playerInsideTrigger = false;
        isWorking = false;
        anim.SetBool("isWorking", false);
    }

    private IEnumerator ProcessItemTransformation(GameObject player)
    {
        Inventory playerInventory = player.GetComponentInChildren<Inventory>();
        if (playerInventory != null && playerInventory.HasItem(itemToTake))
        {
            int count = playerInventory.DeleteItem(itemToTake, level, transform);
            playerInsideTrigger = true;
            isWorking = true;
            anim.SetBool("isWorking", true);

            yield return StartCoroutine(DrawOutlineRoutine(transformationTime));

            for (int i = 0; i < count; i++)
            {
                PrefabManager.Instance.Get(itemToGive.Name, transform.position, transform.rotation).transform.SetParent(transform);
            }
            playerInsideTrigger = false;
            isWorking = false;
            anim.SetBool("isWorking", false);
        }
        else
        {
            if (playerInventory == null) Debug.LogError("플레이어에 PlayerInventory 스크립트가 없습니다!");
            else Debug.Log($"인벤토리에 {itemToTake} 아이템이 없습니다.");
        }
    }

    // ## 수정된 함수: 오브젝트를 (0,0,0)으로 이동 ##
    private IEnumerator DrawOutlineRoutine(float duration)
    {
        if (movingObject == null || progressBarObject == null)
        {
            yield break; // 필요한 컴포넌트가 없으면 코루틴 종료
        }
        
        ResetOutline();
        progressBarObject.SetActive(true);

        float elapsedTime = 0f;
        Vector3 startPosition = movingObject.localPosition; // 현재 위치를 시작점으로 설정
        Vector3 targetPosition = Vector3.zero; // 목표 위치는 로컬 (0,0,0)

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsedTime / duration);

            // Lerp를 사용하여 시작 위치에서 목표 위치로 부드럽게 이동
            movingObject.localPosition = Vector3.Lerp(startPosition, targetPosition, progress);
            
            yield return null;
        }

        // 작업 완료 후 초기화
        ResetOutline();
    }

    // ## 수정된 함수: 오브젝트 위치 초기화 ##
    private void ResetOutline()
    {
        if (progressBarObject != null)
        {
            progressBarObject.SetActive(false);
        }
        if (movingObject != null)
        {
            // 오브젝트 위치를 원래 저장했던 초기 위치로 복원
            movingObject.localPosition = initialObjectPosition;
        }
    }
}