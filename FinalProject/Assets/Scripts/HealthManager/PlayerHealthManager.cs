using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StarterAssets;

public class PlayerHealthManager : HealthManager
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    protected override void Death(GameObject instigator)
    {
        Destroy(gameObject);
        GameManager.instance.GetPlayers().Remove(GetComponent<ThirdPersonController>());
    }

    public override void TakeDamage(float dmg, GameObject instigator)
    {
        base.TakeDamage(dmg, instigator);
    }
}
