using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySwordTrigger : MonoBehaviour
{
    private AIHealthManager aiHealth;

    public void Start()
    {
        aiHealth = GameObject.Find("FemaleCharacterPBR").GetComponent<AIHealthManager>();
    }

    public  void OnTriggerEnter(Collider other)
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
