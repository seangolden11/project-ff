using UnityEngine;

public class AnimalGive : MonoBehaviour
{

    public AnimalType selectedAnimalType; 
    public enum AnimalType { Chicken, Sheep, Cow }
    

    public void SpawnGoods()
    {
        switch (selectedAnimalType)
        {
            case AnimalType.Chicken:
                PrefabManager.Instance.Get("Egg", this.transform.position, this.transform.rotation);
                break;
            case AnimalType.Sheep:
                PrefabManager.Instance.Get("Wool", this.transform.position, this.transform.rotation);
                break;
            default:
                break;

        }
    }
    }
