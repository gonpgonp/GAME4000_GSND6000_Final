using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class CueBall : MonoBehaviour
{
    const float MAX_POWER_DISTANCE = 2.5f;

	public Camera camera_;
	public GameState gameState;
	public GameObject cue;
	public GameObject powerUpHandler;
	public bool hasHit;
	public bool secondTapAvailable;
	public bool cueHitAnyBall = false;
	public bool cueHitMyBall = false;
	public bool hasBroken = false;

	private PlayerInput playerInput;
    private InputAction clickAction;
    private InputAction cancelAction;
    private InputAction pointAction;
	private LineRenderer lr;

	bool seePath;
    float inaccuracy;
    float angerInaccuracy;
    float forceMult = 8.0f;
    bool clickedOnBall;
    bool allBallsStopped;
    bool didScratch;
    float comparisonScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        clickAction = playerInput.currentActionMap.FindAction("Click");
		cancelAction = playerInput.currentActionMap.FindAction("Cancel");
		pointAction = playerInput.currentActionMap.FindAction("Point");

        lr = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameState.state == GameState.States.BILLIARDS)
        {
			CheckAngerInaccuracy();
			CheckClicking();
			Aim();
			//CheckStopped();
		}
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NumberBall"))
        {
            hasBroken = true;
            cueHitAnyBall = true;
        }
    }

    private void CheckAngerInaccuracy()
    {
        float rage = 0.0f;
        if (gameState.isBilliardsP1Turn)
        {
			rage = gameState.p1Rage;
		}
        else
        {
			rage = gameState.p2Rage;
		}

		if (rage > GameState.MINIMUM_FIGHT_RAGE)
		{
			angerInaccuracy = 20.0f * Mathf.Deg2Rad;
		}
        else
        {
            angerInaccuracy = 0.0f;
        }
	}

    private void CheckClicking()
    {
        if (!gameState.isShopOpen && clickAction.WasPressedThisFrame())
        {
            if (!hasHit || secondTapAvailable)
            {
                Vector2 vec = pointAction.ReadValue<Vector2>();
                Vector2 worldVec = camera_.ScreenToWorldPoint(vec);
                if (Vector2.Distance(worldVec, this.transform.position) <= 1.0f)
                {
                    clickedOnBall = true;
                    //Variables.Scene(gameObject.scene).Set("HideButtons", true);
                }
                else
                {
                    clickedOnBall = false;
					//Variables.Scene(gameObject.scene).Set("HideButtons", false);
				}
            }
        }

        if (clickedOnBall && cancelAction.WasPressedThisFrame())
        {
            clickedOnBall = false;
            lr.enabled = false;
			//Variables.Scene(gameObject.scene).Set("HideButtons", false);
		}
    }

    private void Shoot()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
			Vector2 vec = pointAction.ReadValue<Vector2>();
			Vector3 worldVec = camera_.ScreenToWorldPoint(vec);
            Vector2 distanceVec = transform.position - worldVec;
            float distance = Mathf.Min(distanceVec.magnitude, MAX_POWER_DISTANCE);
            float randomAngle = (inaccuracy + angerInaccuracy) * Random.Range(-1, 1);
            float shotAngle = Mathf.Atan2(distanceVec.y, distanceVec.x);
			float x = Mathf.Cos(shotAngle + randomAngle);
            float y = Mathf.Sin(shotAngle + randomAngle);
            Vector2 f = new Vector2(x, y);
            f = f.normalized * distance * forceMult;
            //Vector2 force = distanceVec.normalized * distance * forceMult;
            rb.AddForce(f, ForceMode2D.Impulse);
            if (!hasHit)
            {
                hasHit = true;
                if (!secondTapAvailable)
                {
                    cue.SetActive(false);
                }
            }
            else
            {
                secondTapAvailable = false;
                lr.enabled = false;
				cue.SetActive(false);
			}
        }
	}

    private void Aim()
    {
        if (clickedOnBall)
        {
            lr.enabled = seePath;
            lr.SetPosition(0, transform.position);
			Vector2 vec = pointAction.ReadValue<Vector2>();
			Vector3 worldVec = camera_.ScreenToWorldPoint(vec);
            worldVec.z = 0;
            var thisPos = transform.position;
			thisPos.z = 0;
            Vector3 endPos = (thisPos - worldVec).normalized * 10.0f;
            endPos = endPos + thisPos;
            lr.SetPosition(1, endPos);

			if (clickAction.WasReleasedThisFrame())
			{
                clickedOnBall = false;
                if (lr != null)
                {
                    lr.enabled = false;
                }
                Shoot();
                if (gameState.isBilliardsP1Turn)
                {
                    comparisonScore = gameState.p1BilliardsScore;
                }
                else
                {
					comparisonScore = gameState.p2BilliardsScore;
				}
			}
		}
    }

    private void CheckStopped()
    {
        if (!hasHit || secondTapAvailable) return;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null && rb.linearVelocity.magnitude > 0.1f) return;
        allBallsStopped = true;
        var numBalls = GameObject.FindGameObjectsWithTag("NumberBall");
        foreach (var ball in numBalls)
        {
            Rigidbody2D numRB = ball.GetComponent<Rigidbody2D>();
            if (numRB != null && numRB.linearVelocity.magnitude > 0.1f) allBallsStopped = false;
        }
        if (allBallsStopped)
        {
            hasHit = false;
            cue.SetActive(true);
            powerUpHandler.GetComponent<PowerUpHandler>().ResetPowerUps();
            if (didScratch)
            {
                transform.position = new Vector3(-4.0f, 0.0f, 0.0f);
                didScratch = false;
            }
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag == "Pocket")
        {
            transform.position = new Vector3( 100.0f, 0.0f, 0.0f );
			Rigidbody2D rb = GetComponent<Rigidbody2D>();
			if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                Variables.Application.Set("JustScratched", true);
                didScratch = true;
            }
		}
	}

    public void ReadyForNextTurn()
    {
		hasHit = false;
		cue.SetActive(true);
		powerUpHandler.GetComponent<PowerUpHandler>().ResetPowerUps();
		if (didScratch)
		{
			transform.position = new Vector3(-4.0f, 0.0f, 0.0f);
			didScratch = false;
		}
	}

	public void SetSeePath(bool seePath_)
	{
		seePath = seePath_;
	}

	public void SetSecondTap(bool secondTapAvailable_)
    {
        secondTapAvailable = secondTapAvailable_;
    }

    public void SetInaccuracy(float inaccuracy_)
    {
        inaccuracy = inaccuracy_;
    }

    public void SetMass(float mass_)
    {
        GetComponent<Rigidbody2D>().mass = mass_;
    }
}
