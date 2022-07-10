#pragma warning disable 0649

using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    [SerializeField] private float m_MoveSpeed = 30f;
    [SerializeField] private float m_JumpForce = 400f;							// Amount of force added when the player jumps.
	[Range(0, .3f)] [SerializeField] private float m_MovementSmoothing = .05f;	// How much to smooth out the movement
	[SerializeField] private bool m_AirControl = false;							// Whether or not a player can steer while jumping;
	[SerializeField] private LayerMask m_WhatIsGround;							// A mask determining what is ground to the character
	[SerializeField] private Transform m_GroundCheck;							// A position marking where to check if the player is grounded.
    [SerializeField] private GameObject m_ProjectilePrefab;
    [SerializeField] private Transform m_ProjectileSpawnPoint;
	[SerializeField] private Texture2D m_cursor;
	[SerializeField] private Texture2D m_cursorClick;
	[SerializeField] GameObject m_landingEffect;
	[SerializeField] GameObject m_Wings;

	bool m_hasDoubleJump = true;
	const float k_GroundedRadius = .2f; // Radius of the overlap circle to determine if grounded
	private bool m_Grounded;            // Whether or not the player is grounded.
	private Rigidbody2D m_Rigidbody2D;
	private bool m_FacingRight = true;  // For determining which way the player is currently facing.
	private Vector3 m_Velocity = Vector3.zero;
    private bool m_jump = false;
	private AudioSource m_playerAudio;
	private float m_lastY;
	private float m_startGrav;
	[SerializeField] float m_fallGrav = 1.7f;

	[Header("Events")]
	[Space]

	public UnityEvent OnLandEvent;

	[System.Serializable]
	public class BoolEvent : UnityEvent<bool> { }

	private void Awake()
	{
		m_playerAudio = GetComponent<AudioSource>();
		m_Rigidbody2D = GetComponent<Rigidbody2D>();
		if (OnLandEvent == null)
			OnLandEvent = new UnityEvent();
		m_startGrav = m_Rigidbody2D.gravityScale;
		m_lastY = transform.position.y;
	}

    private void Update()
    {
		if (Time.timeScale == 0) return;

		if(!m_Grounded && m_lastY>transform.position.y)
        {
			m_Rigidbody2D.gravityScale = m_fallGrav;
        }else
        {
			m_Rigidbody2D.gravityScale = m_startGrav;
		}
		m_lastY = transform.position.y;

        if(Input.GetMouseButtonDown(0))
        {
			GameObject gm = Instantiate(m_ProjectilePrefab, m_ProjectileSpawnPoint.position, Quaternion.identity);
			Vector3 screenPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			screenPoint.z = 0;
			gm.transform.right = screenPoint - m_ProjectileSpawnPoint.position;
			m_playerAudio.Play();
			Cursor.SetCursor(m_cursorClick, Vector2.zero, CursorMode.Auto);
			
		}
		if (Input.GetMouseButtonUp(0))
		{
			Cursor.SetCursor(m_cursor, Vector2.zero, CursorMode.Auto);
		}

		if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
			Jump();
        }

        Move(Input.GetAxisRaw("Horizontal")*Time.fixedDeltaTime*m_MoveSpeed,m_jump);
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
				{
					OnLandEvent.Invoke();
					m_hasDoubleJump = true;
					//Destroy(Instantiate(m_landingEffect,transform.position,Quaternion.identity), 3);
				}
					
			}
		}

        
	}

	void Jump()
    {
		if (m_Grounded)
		{
			m_Grounded = false;
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
		}else if (m_hasDoubleJump)
		{
			
			m_hasDoubleJump = false;
			m_Rigidbody2D.velocity = new Vector2(m_Rigidbody2D.velocity.x, 0.1f);
			m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
			Destroy(Instantiate(m_Wings, transform), 0.6f);
        }
	}

	public void Move(float move, bool jump)
	{

		//only control the player if grounded or airControl is turned on
		if (m_Grounded || m_AirControl)
		{
			// Move the character by finding the target velocity
			Vector3 targetVelocity = new Vector2(move * 10f, m_Rigidbody2D.velocity.y);
			// And then smoothing it out and applying it to the character
			m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

			// If the input is moving the player right and the player is facing left...
			if (move > 0 && !m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
			// Otherwise if the input is moving the player left and the player is facing right...
			else if (move < 0 && m_FacingRight)
			{
				// ... flip the player.
				Flip();
			}
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
}
