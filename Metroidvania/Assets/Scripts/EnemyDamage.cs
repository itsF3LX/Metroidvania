using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : GenericDamage
{
    public float maxHealth = 50;
    protected float currentHealth;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void takeDamage(int damage){
        currentHealth -= (float)damage;
        GetComponent<EnemyController2D>().lookingAtPlayer();
        if (currentHealth <= 0){
            die();
        }
    }
    void die(){
        //animation
        //disable
        GetComponent<Collider2D>().enabled = false;
        GetComponent<SpriteRenderer>().enabled = false;
        this.enabled = false;
    }
}