using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SwordTrigger : MonoBehaviour
{

    private PlayerHealthManager playerhealth;
    public void Start()
    {
        playerhealth = GameObject.Find("PlayerArmature").GetComponent<PlayerHealthManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if(other.tag == "Enemy")
        {
            HealthManager enemyHealth = other.GetComponent<HealthManager>();
            if (enemyHealth)
            {
                enemyHealth.TakeDamage(playerhealth.attackDmg, GameObject.Find("PlayerArmature"));
            }
        }
    }
}
