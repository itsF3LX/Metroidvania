using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellarRattleDamage : GenericDamage
{

    public float maxHealth = 50;
    protected float currentHealth;
    float damageReduction = 0.2f;
    bool hiding = false;
    public Animator animator;
    public CellarRattleController2D controller;
    float hideFor = 0f; 
    public Transform attackHitbox;
	public float attackRange = 0.5f;
	public LayerMask enemyLayer;
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time >= hideFor && hiding){
            Invoke("startRunnin", 1f);
            animator.SetTrigger("UnHide");
            hiding = false;
        }
        lookingAtPlayer();
    }

    public override void takeDamage(int damage){
        if (hiding){
            currentHealth -= (float)damage*damageReduction;
            hideFor = Time.time + 5f;
        } else {
            currentHealth -= (float)damage;
            animator.SetTrigger("Hide");
            hiding = true;
            controller.hiding = true;
            hideFor = Time.time + 5f;
        }
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

    void startRunnin(){
        controller.hiding = false;
    }

    public void lookingAtPlayer(){
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackHitbox.position, attackRange,enemyLayer);
		if (hitEnemies != null && hitEnemies.Length > 0){
            controller.halt = true;
		} else {
            controller.halt = false;
        }
	}
}