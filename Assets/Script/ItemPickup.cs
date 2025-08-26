using UnityEngine;
using System.Collections.Generic;
using System.Collections;


public class ItemPickup : MonoBehaviour
{
    public Item item; // 이 오브젝트가 나타내는 아이템

    public float lifeTime = 10f;

    void OnEnable() // 아이템이 활성화될 때 (생성될 때)
    {
        // "N초 뒤에 사라져라" 라는 명령을 예약함
        StartCoroutine(DisappearAfterTime());
    }

    IEnumerator DisappearAfterTime()
    {
        // lifeTime 만큼 기다림
        yield return new WaitForSeconds(lifeTime);

        // 시간이 다 되면 오브젝트를 파괴하거나 비활성화
        PrefabManager.Instance.Release(gameObject);
    }

    

}