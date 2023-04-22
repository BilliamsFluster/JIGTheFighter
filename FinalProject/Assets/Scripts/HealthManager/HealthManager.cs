using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public float health = 100f;
    public float maxHealth = 100f;
    public int attackDmg;

    [SerializeField] HealthBar healthBar;

    void Start()
    {
        healthBar.UpdateHealthBar(health, maxHealth);
    }
    public void DealDamage(GameObject target)
    {
        var atm = target.GetComponent<HealthManager>();
        if(atm != null)
        {
            atm.TakeDamage(attackDmg, target);
          
        }
    }
    public virtual void TakeDamage(float dmg, GameObject instigator) // when we take damage 
    {
        if (health - dmg <= 0)
        {
            health = 0;
            Death(instigator);
            Debug.Log("Object Dead");

        }
        else
        {
            health -= dmg;
            Debug.Log("Taken damage");
        }
        healthBar.UpdateHealthBar(health, maxHealth);
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    protected virtual void Death(GameObject instigator)
    {

    }
}
