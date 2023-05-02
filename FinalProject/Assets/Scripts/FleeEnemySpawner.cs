using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeEnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyToSpawn;
    [SerializeField] private float playerCount;

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayers(); // spawn enemies
        AIHealthManager.OnAIDeath += HandleAIDeath;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void HandleAIDeath(GameObject aiGameObject)
    {

        AIController AI = aiGameObject.GetComponent<AIController>();

        if (AI)
        {
            if (AI.notifySpawnerToRespawn)
            {
                RespawnAI(AI);
                AIHealthManager.OnAIDeath -= HandleAIDeath;

            }
        }
    }

    void RespawnAI(AIController ai)
    {

        SpawnAI(ai);
    }


    public void SpawnPlayers()
    {

        BoxCollider spawnArea = GetComponent<BoxCollider>();
        for (int i = 0; i < playerCount; i++)
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

    void SpawnAI(AIController aiToSpawn = null)
    {
        BoxCollider spawnBoxCollider = gameObject.GetComponent<BoxCollider>();

        // Get the dimensions of the box collider
        Vector3 boxSize = spawnBoxCollider.bounds.size;

        // Calculate the minimum and maximum positions within the collider
        Vector3 minPosition = spawnBoxCollider.bounds.min;
        Vector3 maxPosition = spawnBoxCollider.bounds.max;

        if (aiToSpawn != null)
        {

            Vector3 randomPosition = new Vector3(Random.Range(minPosition.x, maxPosition.x),
                                                    Random.Range(minPosition.y, maxPosition.y),
                                                    Random.Range(minPosition.z, maxPosition.z));

            AIController enemyObj = Instantiate(aiToSpawn, randomPosition, Quaternion.identity);
            AIHealthManager.OnAIDeath += HandleAIDeath;
            AIHealthManager enemy = enemyObj.GetComponent<AIHealthManager>();
            enemy.health = 100f;
            enemy.maxHealth = 100f;

        }

    }
}
