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
                PrefabManager.Instance.InstantiatePrefab("Egg", this.transform.position, this.transform.rotation);
                break;
            default:
                break;

        }
    }
    }
