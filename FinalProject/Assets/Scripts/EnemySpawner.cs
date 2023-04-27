using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float playerCount;
    [SerializeField] private Transform PlayerSpawnLocation;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayers();
        GameManager.instance.SpawnPlayers(PlayerSpawnLocation);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SpawnPlayers()
    {

        BoxCollider spawnArea = GetComponent<BoxCollider>();
        for(int i  = 0; i < playerCount; i++)
        {
            if (spawnArea)
            {
                // Get the dimensions of the box collider
                Vector3 boxSize = spawnArea.bounds.size;

                // Calculate the minimum and maximum positions within the collider
                Vector3 minPosition = spawnArea.bounds.min;
                Vector3 maxPosition = spawnArea.bounds.max;

                Vector3 randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x),
                                                            Random.Range(minPosition.y, maxPosition.y),
                                                            Random.Range(minPosition.z, maxPosition.z));


                Instantiate(enemyToSpawn, randomPosition, Quaternion.identity);

                //Controller newController = GameObject.Find("PlayerArmature").GetComponent<Controller>();
                Cursor.lockState = CursorLockMode.Locked;


                // Wait for a short amount of time to allow the new objects to initialize
            }
        }
        

    }
}
