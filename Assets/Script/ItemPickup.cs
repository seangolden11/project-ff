using UnityEngine;
using System.Collections.Generic;


public class ItemPickup : MonoBehaviour
{
    public Item item; // 이 오브젝트가 나타내는 아이템

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Inventory")) // 플레이어 태그를 확인
        {
            Inventory playerInventory = other.GetComponent<Inventory>();
            if (playerInventory != null)
            {
                if (playerInventory.AddItem(item))
                {
                    gameObject.SetActive(false);
                }
            }
        }
    }
}