using UnityEngine;
using System.Collections.Generic;


[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;

    public Sprite sprite;
    public string Name;
    public int inventoryNum;
    public GameObject prefab; // 3D 인벤토리에 표시될 아이템 프리팹
    public int Size = 3; // 스택 가능 여부 및 최대 스택 수
    public int sellPrice;
    // 기타 아이템 관련 데이터 (예: 효과, 설명 등)
}