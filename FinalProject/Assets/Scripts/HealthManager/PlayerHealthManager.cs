using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerHealthManager : HealthManager
{
    Controller playerController;
    void Start()
    {
        playerController = GetComponent<ThirdPersonController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Death(GameObject instigator)
    {
       
        GameManager.instance.GetPlayers().Remove(GetComponent<ThirdPersonController>());
        GameManager.instance.ActivateGameOverScreen();
        Destroy(gameObject);


    }

    public override void TakeDamage(float dmg, GameObject instigator)
    {
        base.TakeDamage(dmg, instigator);
    }
}
