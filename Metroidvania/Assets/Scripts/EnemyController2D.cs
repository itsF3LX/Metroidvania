using UnityEngine;
using UnityEngine.Events;
using static System.Math;

public class EnemyController2D : MonoBehaviour
{
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;
	[SerializeField] private Transform m_WallCheckFront;
	[SerializeField] private Transform m_GroundCheckFront;							// A position marking where to check if the player is grounded.

	const float k_GroundedRadius = .1f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded = false;            // Whether or not the player is grounded.
	const float k_CeilingRadius = .2f; // Radius of the overlap circle to determine if the player can stand up
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
	public Transform attackHitbox;
	public float attackRange = 0.5f;
	public LayerMask enemyLayer;
	public int damage = 25;
	private float turnAround = 1f;
	private bool goingRight = true;
	private bool wasCollider = true;
	private float radious = 0.3437f;
	public Animator animator;
	private float wait = 1f;
	private Vector2 restart;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_Rigidbody2D = GetComponent<Rigidbody2D>();

		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
	}
	private void FixedUpdate()
	{
		bool wasGrounded = m_Grounded;
		m_Grounded = false;

		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		// This can be done using layers instead but Sample Assets will not overwrite your project settings.
		Collider2D[] colliders = Physics2D.OverlapCircleAll(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);
		for (int i = 0; i < colliders.Length; i++)
		{
			if (colliders[i].gameObject != gameObject)
			{
				m_Grounded = true;
				if (!wasGrounded)
					OnLandEvent.Invoke();
			}
		}
		if (m_Grounded){
			Collider2D[] collidersFrontWall = Physics2D.OverlapCircleAll(m_WallCheckFront.position, radious, m_WhatIsGround);
			if (collidersFrontWall.Length > 0) {
				if (wasCollider){
					if (goingRight){
						animator.SetFloat("speed", 0);
						wait = 0;
						Invoke("waitingL", 1);
					} else {
						animator.SetFloat("speed", 0);
						wait = 0;
						Invoke("waiting", 1);
					}
				}
			} else {
				Collider2D[] collidersFront = Physics2D.OverlapCircleAll(m_GroundCheckFront.position, k_GroundedRadius, m_WhatIsGround);
				for (int i = 0; i < collidersFront.Length; i++)
				{
					if (collidersFront[i].gameObject != gameObject)
					{
						if (goingRight){
							turnAround = 1f;
							wasCollider = true;
						} else {
							turnAround = -1f;
							wasCollider = true;
						}
					} 
				}
				if (collidersFront.Length == 0 && wasCollider){
					if (goingRight){
						wait = 0;
						Invoke("waitingL", 1);
						animator.SetFloat("speed", 0);
					} else {
						wait = 0;
						Invoke("waiting", 1);
						animator.SetFloat("speed", 0);
					}
				}
			}
		}
	}


	public void Move(float move, bool attacking, bool canmove)
	{
		// If crouching, check to see if the character can stand up
		//only control the player if grounded or airControl is turned on
		if (canmove){
			if (m_Grounded || m_AirControl)
			{
				animator.SetFloat("speed", move);
				// Move the character by finding the target velocity
				Vector3 targetVelocity = new Vector2(move * 4f * turnAround * wait, m_Rigidbody2D.velocity.y);
				// And then smoothing it out and applying it to the character
				m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

				// If the input is moving the player right and the player is facing left...
				if (turnAround > 0 && !m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
				// Otherwise if the input is moving the player left and the player is facing right...
				else if (turnAround < 0 && m_FacingRight)
				{
					// ... flip the player.
					Flip();
				}
			} else {
				animator.SetFloat("speed", 0);
			}
		} else if (attacking) {
			m_Rigidbody2D.velocity = Vector3.zero;
			animator.SetFloat("speed", 0);
			attack();
		} else {
			animator.SetFloat("speed", 0);
		}
	}


	private void Flip()
	{
		// Switch the way the player is labelled as facing.
		m_FacingRight = !m_FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	public bool GetM_Grounded(){
		return m_Grounded;
	}

	public void attack(){
		//play animation

		//detect enemys in range
		Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackHitbox.position, attackRange,enemyLayer);
		//apply damage
		animator.SetTrigger("Attack");
		foreach(Collider2D enemy in hitEnemies){
			enemy.GetComponent<PlayerHealth>().takeDamage(damage);
		}
	}

	void OnDrawGizmosSelected(){
		if (attackHitbox == null){
			return;
		}
		Gizmos.DrawWireSphere(attackHitbox.position,attackRange);
	}

	public void waiting(){
		turnAround = 1f;
		goingRight = true;
		wasCollider = false;
		wait = 1;
	}

	public void waitingL(){
		turnAround = -1f;
		goingRight = false;
		wasCollider = false;
		wait = 1;
	}
}

