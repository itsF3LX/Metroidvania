using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellarRattleMovement : MonoBehaviour
{
    public CellarRattleController2D controller;
    public float runSpeed = 40f;
    protected Rigidbody2D body; 
    //Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        body.velocity = body.velocity;
    }

    void FixedUpdate(){
        //Movement
        controller.Move(runSpeed * Time.fixedDeltaTime);
    }
}