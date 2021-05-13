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
    bool changePlatformState = false;
    public GameObject platform;
    bool attacking = false;
    Rigidbody2D body; 
    public float attackRate = 1f;
	float nextAttackTime = 0f;  
    public Transform interactHitbox;
	public float interactRange = 0.5f;
	public LayerMask interactableLayer;
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
            if (Input.GetKey("s")){
                PlatformEffector2D effector = platform.GetComponent<PlatformEffector2D>();
                effector.surfaceArc = 0;
            } else {
                jump = true;
            }
        }
        if (Input.GetKeyUp("s")){
            changePlatformState = true;
        } 
        if (!this.GetComponent<Collider2D>().Distance(platform.GetComponent<Collider2D>()).isOverlapped && changePlatformState){
            PlatformEffector2D effector = platform.GetComponent<PlatformEffector2D>();
            effector.surfaceArc = 180;
            changePlatformState = false;
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
        if (Time.time >= nextAttackTime){
            if (Input.GetMouseButtonDown(0)){
                attacking = true;
                nextAttackTime = Time.time + 1f/attackRate;
            }
        }
        if (Input.GetButtonDown("Interact")){
            Collider2D[] interact = Physics2D.OverlapCircleAll(interactHitbox.position, interactRange,interactableLayer);
            foreach(Collider2D inter in interact){
			    inter.GetComponent<Interactable>().disappear();
		    }
        }
    }

    void FixedUpdate(){
        //Movement
        controller.Move(horizontalMove * Time.fixedDeltaTime, crouch,jump, attacking);
        attacking = false;
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
