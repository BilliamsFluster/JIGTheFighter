using StarterAssets;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private AudioSource swordSlash;
    public AudioClip swordSound;

    [SerializeField] public GameObject playerToSpawn;
    private List<ThirdPersonController> players;
    private List<AIController> enemies;


    public void Start()
    {
        swordSlash = GetComponent<AudioSource>();
        players = new List<ThirdPersonController>();
        enemies = new List<AIController>();
        SpawnPlayers();
    }
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
    }
    

    public void SpawnPlayers()
    {
        
        BoxCollider spawnArea = GetComponent<BoxCollider>();
        
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


            Instantiate(playerToSpawn, randomPosition, Quaternion.identity);

            //Controller newController = GameObject.Find("PlayerArmature").GetComponent<Controller>();
            Cursor.lockState = CursorLockMode.Locked;


            // Wait for a short amount of time to allow the new objects to initialize
        }
        
    }


    public void PlaySwordSlash()
    {
        swordSlash.PlayOneShot(swordSound);
        Debug.Log("Sound");
    }
}
