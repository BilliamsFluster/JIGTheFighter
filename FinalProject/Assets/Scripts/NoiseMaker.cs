using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseMaker : MonoBehaviour
{
    [SerializeField] private AudioSource source = null;
    [SerializeField] private float soundRange = 25f;

    private void Start()
    {
        source = GameManager.instance.swordSlash;

    }
    private void OnMouseDown()
    {
        var sound = new Sound(transform.position, soundRange);

        Sounds.MakeSound(sound);
    }
}
