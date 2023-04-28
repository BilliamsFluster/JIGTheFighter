using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIHealthManager : HealthManager
{
    
    public delegate void AIDeathEventHandler(GameObject aiGameObject);
    public static event AIDeathEventHandler OnAIDeath;
    public int deathScoreReward = 10;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected override void Death(GameObject instigator)
    {
        Controller player = instigator.GetComponent<Controller>();

        if (player != null)
        {
            if (player != null)
            {
                player.AddScore(deathScoreReward);
                HealthManager playerHealth = player.GetComponent<HealthManager>();
                if(playerHealth != null)
                {
                    playerHealth.SetHealth(playerHealth.maxHealth);
                    GameManager.instance.playerScore = player.GetScore();
                }
            }
            if (OnAIDeath != null)
            {
                OnAIDeath(gameObject);
            }
            

        }
        Destroy(gameObject);
        GameManager.instance.GetEnemies().Remove(GetComponent<AIController>());
    }

    public override void TakeDamage(float dmg, GameObject instigator)
    {
        base.TakeDamage( dmg, instigator);
    }
}
