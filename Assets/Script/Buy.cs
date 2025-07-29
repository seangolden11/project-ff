using Unity.Mathematics;
using UnityEngine;

public class Buy : MonoBehaviour
{
    public enum AnimalType {Chicken,Sheep,Cow };
    public AnimalType type;

    private int price;

    void Start()
    {
        switch (type)
        {
            case AnimalType.Chicken:
                price = 100;
                break;
            default:
                break;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (MoneyManager.Instance.TryRemoveMoney(price))
            {
                PrefabManager.Instance.InstantiatePrefab(type.ToString(), Vector3.up, quaternion.identity);
            }
        }
    }
}
