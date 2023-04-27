using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class ScoreText : MonoBehaviour
{
    public Text text;
    [SerializeField] private GameObject Character;
    ThirdPersonController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = Character.GetComponent<ThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        DisplayScore();
    }

    private void DisplayScore()
    {

        controller = Character.GetComponent<ThirdPersonController>(); 
        if (controller != null)
        {

            int? score = controller.GetScore();
            if (score != null)
            {
                text.text = score.ToString();
            }
            
        }
    }
}
