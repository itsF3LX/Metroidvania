using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{   

    public EnemyController2D controller;
    float horizontalMove = 0f;
    public float runSpeed = 40f;
    bool attacking = false;
    Rigidbody2D body; 
    public float attackRate = 1f;
	float nextAttackTime = 0f;  
    //Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove =  1 * runSpeed;
        body.velocity = body.velocity;
        // body.velocity = Vector2.zero;
        if (Time.time >= nextAttackTime){
            //if (Input.GetMouseButtonDown(0)){
            if (false){
                attacking = true;
                nextAttackTime = Time.time + 1f/attackRate;
            }
        }
    }

    void FixedUpdate(){
        //Movement
        controller.Move(horizontalMove * Time.fixedDeltaTime, attacking);
        attacking = false;
    }
}

