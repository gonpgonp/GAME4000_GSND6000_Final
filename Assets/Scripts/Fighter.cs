using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Fighter : MonoBehaviour
{
    public Vector2 startPosition;
    public GameObject opponent;

	private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    private AudioClip punchClip;
    private AudioClip guardClip;

	private PlayerInput playerInput;
	private InputAction moveAction;
	private InputAction attackAction;
	private InputAction guardAction;

	private int playerNum = 0;
    private float moveSpeed = 15.0f;
    private float knockBackStrength = 80.0f;
    private float attackKnockback = -30.0f;
    private float guardKnockback = -40.0f;
    private int invulnTimer = 0;
    private int invulnDuration = 25;
    private int hitstunTimer = 0;
    private int hitstunDuration = 20;
    private Vector2 moveVector = Vector2.zero;
    private bool isAttacking = false;
	private bool isGuarding = false;
	private float movementForce = 80.0f;
    private float maxSpeed = 20.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        punchClip = Resources.Load<AudioClip>("Assets/Sounds/SFX/punch_2.wav");
        guardClip = Resources.Load<AudioClip>("Assets/Sounds/SFX/wood.wav");

		playerInput = GetComponent<PlayerInput>();
		moveAction = playerInput.currentActionMap.FindAction("Move");
		attackAction = playerInput.currentActionMap.FindAction("Attack");
		guardAction = playerInput.currentActionMap.FindAction("Guard");
		if (gameObject.name == "Player1")
        {
            playerNum = 1;
		}
        else
        {
            playerNum = 2;
        }
    }

    // Update is called once per frame
    void Update()
    {
        FaceOpponent();
        DoMovement();
        DoAttacks();
        DoGuards();
    }

    void FaceOpponent()
    {
        if (opponent.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-2, 2, 2);
        }
        else if (opponent.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(2, 2, 2);
        }
    }

    void DoMovement()
    {
        if (hitstunTimer != 0) return;
        if (moveAction.IsPressed()) 
        {
            moveVector = moveAction.ReadValue<Vector2>();
            animator.SetBool("DoRun", true);
        }
        else if (moveAction.WasReleasedThisFrame())
        {
            moveVector = Vector2.zero;
            animator.SetBool("DoRun", false);
        }

        rb.AddForce(moveVector);
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    void DoAttacks()
    {
        if (hitstunTimer != 0) return;
        if (attackAction.WasPressedThisFrame())
        {
            animator.Play("Attack");
        }
    }

    void DoGuards()
    {
        if (hitstunTimer != 0) return;
        if (guardAction.WasPressedThisFrame())
        {
            animator.Play("Guard");
        }
    }

    void PlayGuardSound()
    {
		audioSource.clip = guardClip;
		audioSource.volume = 1.0f;
		audioSource.time = 0.09f;
		audioSource.Play();
	}

    void PlayPunchSound()
    {
		audioSource.clip = punchClip;
		audioSource.volume = 0.5f;
		audioSource.Play();
	}

	public void DoReset()
	{
        rb.linearVelocity = Vector2.zero;
        transform.position = startPosition;
		animator.Play("Idle");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        float facingMult = collision.transform.position.x < transform.position.x ? -1f : 1f;
        if (isGuarding)
        {
            PlayGuardSound();
        }
		if (!isGuarding && invulnTimer == 0)
        {
            invulnTimer = invulnDuration;
            hitstunTimer = hitstunDuration;
            rb.AddForce(new Vector2(knockBackStrength * facingMult, 0.0f), ForceMode2D.Impulse);
            animator.Play("GotHit");
        }
	}
}
