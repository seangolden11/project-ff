using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{

    public Transform wellspot;
    
    
  
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        GameObject well = PrefabManager.Instance.InstantiatePrefab("well",wellspot);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
