using StarterAssets;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private AudioSource swordSlash;
    public AudioClip swordSound;
    public bool gameOver;
    public float swordVolume = 0f;

    [SerializeField] public GameObject playerToSpawn;
    [SerializeField] private List<ThirdPersonController> players;
    [SerializeField] private List<AIController> enemies;


    //Game States

    //[SerializeField] private GameObject TitleSccreenStateObject;
    //[SerializeField] private GameObject MainMenuStateObject;
    //[SerializeField] private GameObject OptionsScreenStateObject;
    //[SerializeField] private GameObject CreditsScreenStateObject;
    //[SerializeField] private GameObject GameplayStateObject;
    //[SerializeField] private GameObject GameOverScreenStateObject;
    //[SerializeField] private GameObject GameOverScreen;



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
        if (SceneManager.GetActiveScene().name != "Level")
        {
           
        }
        else
        {
           
            
        }

       
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

        //Controller newController = GameObject.Find("PlayerArmature").GetComponent<Controller>();
        Cursor.lockState = CursorLockMode.Locked;

    }


    public void PlaySwordSlash()
    {
        swordSlash.PlayOneShot(swordSound,swordVolume);
    }

    public void ActivateMainMenuScreen()
    {

        
        gameOver = false;

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
    public void ActivateGameOverScreen()
    {

        //GameOverScreen.SetActive(true);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        gameOver = true;

    }


}
