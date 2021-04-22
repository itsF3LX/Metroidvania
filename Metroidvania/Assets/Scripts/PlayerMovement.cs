using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{   

    public CharacterController2D controller;
    float horizontalMove = 0f;
    public float runSpeed = 40f;
    public float dashSpeed;
    bool jump = false;
    bool crouch = false;
    public float dashDuration;
    public float dashTimer;
    private bool canDash = true;
    Rigidbody2D body;   
    enum DashDirection
    {
        Left,
        Right,
        NoDirection
    }
    private DashDirection dashDirection = DashDirection.NoDirection;
    //Start is called before the first frame update
    void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        horizontalMove =  Input.GetAxisRaw("Horizontal") * runSpeed;
        body.velocity = body.velocity;
        // body.velocity = Vector2.zero;
        if(Input.GetButtonDown("Jump")){
            jump = true;
        }
        if(Input.GetButtonDown("Crouch")){
            crouch = true;
        } else if (Input.GetButtonUp("Crouch")){
            crouch = false;
        }
        if (canDash){
            if (Input.GetButtonDown("Dash")){
                if (Input.GetAxisRaw("Horizontal") > 0){
                    dashDirection = DashDirection.Right; 
                    canDash = false;
                } else if (Input.GetAxisRaw("Horizontal") < 0){
                    dashDirection = DashDirection.Left;
                    canDash = false;
                }
            }
        }
    }

    void FixedUpdate(){
        //Movement
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch,jump);
        if (dashDirection != DashDirection.NoDirection) {
            if (dashTimer >= dashDuration) {
                dashDirection = DashDirection.NoDirection;
                dashTimer = 0;
                body.velocity = body.velocity;
            } else {
                dashTimer += Time.deltaTime;
                if (dashDirection == DashDirection.Left) {
                    body.velocity = Vector2.left * dashSpeed;
                }

                if (dashDirection == DashDirection.Right) {
                    body.velocity = Vector2.right * dashSpeed;
                }
            }
        }
        if (dashTimer == 0 && controller.GetM_Grounded()){
            canDash = true;
        }
        jump = false;
    }
}
