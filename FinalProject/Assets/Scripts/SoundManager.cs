using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{

    [SerializeField] Slider gameVolumeSlider;

    [SerializeField] Slider swordVolumeSlider;
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            PlayerPrefs.SetFloat("musicVolume", 1);
        }
        else
        {
            Load();
        }
    }

    public void ChangeGameVolume()
    {
        AudioListener.volume = gameVolumeSlider.value;
        Save();
    }
    public void ChangeSwordVolume()
    {
        if (swordVolumeSlider != null)
        {
            if(GameManager.instance != null)
            {
                GameManager.instance.swordVolume = swordVolumeSlider.value;
                //Save();
            }

        }
        Debug.Log("swordVolumeSlider: " + swordVolumeSlider);

    }

    private void Load()
    {
        PlayerPrefs.GetFloat("musicVolume");
        PlayerPrefs.GetFloat("swordMusicVolume");
    }
    private void Save()
    {
        PlayerPrefs.SetFloat("musicVolume", gameVolumeSlider.value);
        PlayerPrefs.SetFloat("swordMusicVolume", swordVolumeSlider.value);
    }
}
