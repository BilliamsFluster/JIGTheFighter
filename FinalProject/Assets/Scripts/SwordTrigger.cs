using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class SwordTrigger : MonoBehaviour
{

    private StarterAssetsInputs inputs;
    private PlayerHealthManager playerhealth;
    public void Start()
    {
        inputs = GameObject.Find("PlayerArmature").GetComponent<StarterAssetsInputs>();
        playerhealth = GameObject.Find("PlayerArmature").GetComponent<PlayerHealthManager>();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Debug.Log(inputs.attacking);
            if (inputs.attacking)
            {
                HealthManager enemyHealth = other.GetComponent<HealthManager>();
                if (enemyHealth)
                {
                    enemyHealth.TakeDamage(playerhealth.attackDmg, gameObject);
                }
            }
        }
       
    }
}
