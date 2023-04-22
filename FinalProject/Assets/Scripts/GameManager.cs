using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public AudioSource swordSlash;
    public AudioClip swordSound;
    public void Start()
    {
        swordSlash = GetComponent<AudioSource>();
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

    public void PlaySwordSlash()
    {
        swordSlash.PlayOneShot(swordSound);
    }
}
