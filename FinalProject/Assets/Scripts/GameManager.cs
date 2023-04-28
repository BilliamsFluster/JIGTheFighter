using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioSource swordSlash;
    public AudioClip swordSound;
    public bool gameOver;
    public float swordVolume = 0f;
    public int playerScore = 0;

    [SerializeField] public GameObject playerToSpawn;
    [SerializeField] private List<ThirdPersonController> players;
    [SerializeField] private List<AIController> enemies;


    //Game States
    private GameObject GameOverScreen;



    public List<ThirdPersonController> GetPlayers()
    {
        return players;
    }
    public List<AIController> GetEnemies()
    {
        
        return enemies;
    }
    


    public void Start()
    {
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
        swordSlash = GetComponent<AudioSource>();
        players = new List<ThirdPersonController>();
        enemies = new List<AIController>();
    }
   

    public void SpawnPlayers(Transform T)
    {
        Instantiate(playerToSpawn, T.position, Quaternion.identity);
        Cursor.lockState = CursorLockMode.Locked;
        GameOverScreen = GameObject.Find("GameOverScreen1");
        GameOverScreen.SetActive(false);
    }


    public void PlaySwordSlash()
    {
        swordSlash.PlayOneShot(swordSound,swordVolume);
    }

   

   
    public void Restart()
    {
        if (SceneManager.GetActiveScene().name == "Level")
        {
            OpenLevel();

        }
    }

    public void OpenLevel()
    {
        SceneManager.LoadScene("Level");
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public int GetPlayerScore()
    {
        return playerScore;
    }
    public void ActivateGameOverScreen()
    {

        GameOverScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameOver = true;

    }


}
