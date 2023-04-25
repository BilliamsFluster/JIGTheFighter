using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordTrigger : SwordTrigger
{
    private AIHealthManager aiHealth;
    public void Start()
    {
        aiHealth = GameObject.Find("FemaleCharacterPBR").GetComponent<AIHealthManager>();
    }

    public override void OnTriggerEnter(Collider other)
    {

        if (other.tag == "Player")
        {
            HealthManager playerHealth = other.GetComponent<HealthManager>();
            if (playerHealth)
            {
                playerHealth.TakeDamage(aiHealth.attackDmg, gameObject);
            }
        }





    }
}
