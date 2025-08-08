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
                case AnimalType.Sheep:
                price = 1000;
                break;
                case AnimalType.Cow:
                price = 10000;
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
                PrefabManager.Instance.Get(type.ToString(), Vector3.up, quaternion.identity);
            }
        }
    }
}
