using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{   

    public EnemyController2D controller;
    public float runSpeed = 40f;
    bool attacking = false;
    Rigidbody2D body; 
    public float attackRate = 1f;
	float nextAttackTime = 0f;  
    public Transform attackHitbox;
	public float attackRange = 0.5f;
    [SerializeField] private LayerMask whatIsPlayer;
    bool canmove = true;
    //Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = body.velocity;
        if (Time.time >= nextAttackTime){
            canmove = true;
            Collider2D[] colliders = Physics2D.OverlapCircleAll(attackHitbox.position, attackRange, whatIsPlayer);
            for (int i = 0; i < colliders.Length; i++){
			    if (colliders[i].gameObject != gameObject){
				    attacking = true;
                    nextAttackTime = Time.time + 1f/attackRate;
                    canmove = false;
                    Debug.Log("Hit!");
			    }
		    }
        }
    }

    void FixedUpdate(){
        //Movement
        controller.Move(runSpeed * Time.fixedDeltaTime, attacking, canmove);
        attacking = false;
    }
}

