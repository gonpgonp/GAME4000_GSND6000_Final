using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class CueBall : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction clickAction;
    private InputAction cancelAction;
    private InputAction pointAction;

    public Camera camera;
    public GameObject cue;
	public GameObject powerUpHandler;
	bool secondTapAvailable;
    float inaccuracy;
    float angerInaccuracy;

    float forceMult = 8.0f;
    bool clickedOnBall;
    bool hasHit;
    bool allBallsStopped;
    bool didScratch;
    float comparisonScore;

    public bool cueHitAnyBall = false;
    public bool cueHitMyBall = false;
    public bool hasBroken = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        clickAction = playerInput.currentActionMap.FindAction("Click");
		cancelAction = playerInput.currentActionMap.FindAction("Cancel");
		pointAction = playerInput.currentActionMap.FindAction("Point");
    }

    // Update is called once per frame
    void Update()
    {
		CheckAngerInaccuracy();
		CheckClicking();
        Aim();
        CheckStopped();
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
		if (Variables.Application.Get<bool>("BilliardsIsP2Turn") && Variables.Application.Get<int>("P2Rage") > Variables.Application.Get<int>("MinimumFightRage"))
		{
			angerInaccuracy = 10.0f * Mathf.Deg2Rad;
		}
		else if (Variables.Application.Get<int>("P1Rage") > Variables.Application.Get<int>("MinimumFightRage"))
		{
			angerInaccuracy = 0.0f;
		}
	}

    private void CheckClicking()
    {
        if (!Variables.Scene(gameObject.scene).Get<bool>("ShopOpen") && !Variables.Scene(gameObject.scene).Get<bool>("isFight") && clickAction.WasPressedThisFrame())
        {
            if (!hasHit || secondTapAvailable)
            {
                Vector2 vec = pointAction.ReadValue<Vector2>();
                Vector2 worldVec = camera.ScreenToWorldPoint(vec);
                if (Vector2.Distance(worldVec, this.transform.position) <= 1.0f)
                {
                    clickedOnBall = true;
                    Variables.Scene(gameObject.scene).Set("HideButtons", true);
                }
                else
                {
                    clickedOnBall = false;
                }
            }
        }

        if (clickedOnBall && cancelAction.WasPressedThisFrame())
        {
            clickedOnBall = false;
        }
    }

    private void Shoot()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
			Vector2 vec = pointAction.ReadValue<Vector2>();
			Vector3 worldVec = camera.ScreenToWorldPoint(vec);
            Vector2 distanceVec = transform.position - worldVec;
            float distance = Mathf.Min(distanceVec.magnitude, Variables.Application.Get<float>("MaxCueDistance"));
            Vector2 force = distanceVec.normalized * distance * forceMult;
            rb.AddForce(force, ForceMode2D.Impulse);
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
				cue.SetActive(false);
			}
        }
	}

    private void Aim()
    {
        if (clickedOnBall)
        {
            LineRenderer lr = GetComponent<LineRenderer>();
            if (lr != null)
            {
                lr.enabled = Variables.Application.Get<bool>("SeePath");
                lr.SetPosition(0, transform.position);
				Vector2 vec = pointAction.ReadValue<Vector2>();
				Vector3 worldVec = camera.ScreenToWorldPoint(vec);
                Vector2 endPos = (transform.position - worldVec).normalized * 10.0f;
                lr.SetPosition(1, endPos);
            }

			if (clickAction.WasReleasedThisFrame())
			{
                clickedOnBall = false;
                if (lr != null)
                {
                    lr.enabled = false;
                }
                Shoot();
                if (Variables.Application.Get<bool>("BilliardsIsP2Turn"))
                {
                    comparisonScore = Variables.Application.Get<int>("P2_Score");
                }
                else
                {
					comparisonScore = Variables.Application.Get<int>("P1_Score");
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
            Variables.Scene(gameObject.scene).Get<bool>("TriggerSwapBilliardsTurns");
            hasHit = false;
            Variables.Scene(gameObject.scene).Set("HideButtons", false);
            cue.SetActive(true);
            // set poweruphandler's newTurn = true
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
}
