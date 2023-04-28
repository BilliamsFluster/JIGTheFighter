using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WidgetManager : MonoBehaviour
{
    public GameObject TitleSccreenStateObject;
    public GameObject MainMenuStateObject;
    public GameObject OptionsScreenStateObject;
    public GameObject CreditsScreenStateObject;
    public GameObject GameplayStateObject;


    // Start is called before the first frame update
    void Start()
    {
        ActivateMainMenuScreen();

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void DeactivateAllStates()
    {
        TitleSccreenStateObject.SetActive(false);
        MainMenuStateObject.SetActive(false);
        OptionsScreenStateObject.SetActive(false);
        CreditsScreenStateObject.SetActive(false);
        GameplayStateObject.SetActive(false);
    }
    public void ActivateTitleScreen()
    {
        DeactivateAllStates();
        TitleSccreenStateObject.SetActive(true);
    }
    public void ActivateMainMenuScreen()
    {
        DeactivateAllStates();
        MainMenuStateObject.SetActive(true);
        // gameOver = false;

    }
    public void ActivateOptionsScreen()
    {
        DeactivateAllStates();
        OptionsScreenStateObject.SetActive(true);

    }
    public void ActivateCreditsScreen()
    {
        DeactivateAllStates();
        CreditsScreenStateObject.SetActive(true);

    }
    public void ActivateGameplayStateObject()
    {
        DeactivateAllStates();
        GameplayStateObject.SetActive(true);

    }

    public void OpenLevel()
    {
        SceneManager.LoadScene("Level");
    }
    public void QuitApp()
    {
        Application.Quit();
        Debug.Log("Quit App");
    }


}
