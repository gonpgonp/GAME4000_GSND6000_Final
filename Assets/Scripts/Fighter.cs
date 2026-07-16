using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class Fighter : MonoBehaviour
{
    public Vector2 startPosition;
    public GameObject opponent;
    public GameState gameState;
    public FightUI fightUI;
    public int index;

	private Rigidbody2D rb;
    private Animator animator;
    private AudioSource audioSource;
    public AudioClip punchClip;
    public AudioClip guardClip;

	private PlayerInput playerInput;
	private InputAction moveAction;
	private InputAction attackAction;
	private InputAction guardAction;

    private bool facingLeft = false;
    private float knockBackStrength = 80.0f;
    private float guardKnockback = 40.0f;
    private int invulnTimer = 0;
    private int invulnDuration = 25;
    private int hitstunTimer = 0;
    private int hitstunDuration = 20;
    private Vector2 moveVector = Vector2.zero;
    private bool inactionable = false;
	private bool isGuarding = false;
	private float movementForce = 80.0f;
    private float maxSpeed = 20.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        //punchClip = Resources.Load<AudioClip>("Assets/Sounds/SFX/punch_2.wav");
        //guardClip = Resources.Load<AudioClip>("Assets/Sounds/SFX/wood.wav");

		playerInput = GetComponent<PlayerInput>();
		moveAction = playerInput.currentActionMap.FindAction("Move");
		attackAction = playerInput.currentActionMap.FindAction("Attack");
		guardAction = playerInput.currentActionMap.FindAction("Guard");
    }

    // Update is called once per frame
    void Update()
    {
        FaceOpponent();
        DoAttacks();
        DoGuards();
    }

	private void FixedUpdate()
	{
		DoMovement();
        DecrementTimers();
	}

	void FaceOpponent()
    {
        if (hitstunTimer != 0 || inactionable) return;
        if (opponent.transform.position.x < transform.position.x)
        {
            transform.localScale = new Vector3(-2, 2, 2);
            facingLeft = true;
		}
        else if (opponent.transform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(2, 2, 2);
            facingLeft = false;
		}
    }

    void DoMovement()
    {
        if (hitstunTimer != 0 || isGuarding) return;
        if (moveAction.IsPressed()) 
        {
            moveVector = moveAction.ReadValue<Vector2>();
            animator.SetBool("DoRun", true);
        }
        else //if (moveAction.WasReleasedThisFrame())
        {
            moveVector = Vector2.zero;
            animator.SetBool("DoRun", false);
        }

        rb.AddForce(moveVector * movementForce);
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    void DoAttacks()
    {
        if (hitstunTimer != 0 || inactionable) return;
        if (attackAction.WasPressedThisFrame())
        {
            animator.Play("Attack");
        }
    }

    void DoGuards()
    {
        if (hitstunTimer != 0 || inactionable) return;
        if (guardAction.WasPressedThisFrame())
        {
            animator.Play("Guard");
        }
    }

    void DecrementTimers()
    {
        if (hitstunTimer > 0)
        {
			hitstunTimer -= 1;
		}
        if (invulnTimer > 0)
        {
            invulnTimer -= 1;
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

    private void TakeHit(Collider2D collision)
    {
        Debug.Log(gameObject.name + " : TakeHit");
		float facingMult = facingLeft ? 1f : -1f;
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
            inactionable = false;
		}
	}

    private void DoHit(Collider2D collision)
    {
		Debug.Log(gameObject.name + " : DoHit");
		float facingMult = facingLeft ? 1f : -1f;
        Fighter def = collision.gameObject.GetComponentInParent<Fighter>();
        if (def == null) return;
        if (def.isGuarding)
        {
            hitstunTimer = 50;
			rb.AddForce(new Vector2(guardKnockback * facingMult, 0.0f), ForceMode2D.Impulse);
            animator.Play("GotParried");
		}
        else
        {
            PlayPunchSound();
            // add points
            if (index == 1)
            {
                gameState.p1FightScore = Mathf.Min(5, gameState.p1FightScore + 1);
                gameState.p2FightScore = Mathf.Max(0, gameState.p2FightScore - 1);
            }
            else if (index == 2)
            {
				gameState.p2FightScore = Mathf.Min(5, gameState.p2FightScore + 1);
				gameState.p1FightScore = Mathf.Max(0, gameState.p1FightScore - 1);
			}
			fightUI.SetScoreUI();
		}
    }
    public void SetGuarding(int input)
    {
        isGuarding = input != 0;
    }

    public void SetInaction(int input)
    {
        inactionable = input != 0;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.gameObject.layer == 10) //collided with hurtbox (hit opponent)
        {
            DoHit(collision);
        }
        if (collision.gameObject.layer == 11) //collided with hitbox (got hit by opponent)
        {
            TakeHit(collision);
        }
        
	}
}
