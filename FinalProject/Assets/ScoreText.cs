using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using StarterAssets;

public class ScoreText : MonoBehaviour
{
    public Text text;
    
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DisplayScore();
    }

    private void DisplayScore()
    {

        int score = GameManager.instance.GetPlayerScore();
        text.text = score.ToString();
        
           
        
    }
}
