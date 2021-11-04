using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnZombie : MonoBehaviour
{
    public GameObject zombiePrefabs;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Spawning Zombie");
            Instantiate(zombiePrefabs, transform.position, Quaternion.identity);
        }
    }

}
