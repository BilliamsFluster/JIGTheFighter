using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Controller : MonoBehaviour
{

    [SerializeField] protected GameObject swordObj;
    BoxCollider sword;
    protected int playerScore;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void AddScore(int score)
    {
        playerScore += score;
    }


    protected virtual void AttackStart()
    {
        sword = swordObj.GetComponent<BoxCollider>();
        if(sword != null)
        {
            sword.isTrigger = true;
        }
    }

    protected virtual void AttackEnd()
    {
        sword = swordObj.GetComponent<BoxCollider>();
        if (sword != null)
        {
            sword.isTrigger = false;
        }
    }

    public int GetScore()
    {
        return playerScore;
    }
}
