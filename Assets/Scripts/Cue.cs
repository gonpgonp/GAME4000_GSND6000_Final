using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D;
public class Cue : MonoBehaviour
{
	const float MAX_POWER_DISTANCE = 2.5f;

	public GameObject targetBall;
	public PowerUpHandler powerUpHandler;
	public float maxCueDistance = 2.5f;

	public bool hasHit;
	public bool secondTapAvailable;
	public bool cueHitAnyBall = false;
	public bool cueHitMyBall = false;
	public bool hasBroken = false;


	private bool canShoot = true;
	private float noShootTimer = 0.0f;
	private bool seePath = false;
	private float inaccuracy = 0.0f;
	private float angerInaccuracy = 0.0f;
	private float forceMult = 8.0f;
	private bool clickedOnBall = false;

	private PlayerInput playerInput;
	private InputAction clickAction;
	private InputAction cancelAction;
	private InputAction pointAction;
	private LineRenderer lineRenderer;

    private SpriteRenderer spriteRenderer;

    
    void Start()
    {
		playerInput = GetComponent<PlayerInput>();
		clickAction = playerInput.currentActionMap.FindAction("Click");
		cancelAction = playerInput.currentActionMap.FindAction("Cancel");
		pointAction = playerInput.currentActionMap.FindAction("Point");

		lineRenderer = GetComponent<LineRenderer>();
		spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
		if (GameState.state == GameState.States.BILLIARDS)
		{
			DoCueVisual();
			CheckAngerInaccuracy();
			CheckCanShoot();
			CheckClicking();
			Aim();
		}
	}

	public void ReadyForNextTurn()
	{
		hasHit = false;
		spriteRenderer.enabled = true;
		powerUpHandler.GetComponent<PowerUpHandler>().ResetPowerUps();
	}

	public void SetSeePath(bool seePath_)
	{
		seePath = seePath_;
	}

	public void SetSecondTap(bool secondTapAvailable_)
	{
		secondTapAvailable = secondTapAvailable_;
	}

	public void SetTargetBall(GameObject targetBall_)
	{
		targetBall.GetComponent<NumberBall>().isTargetBall = false;
		targetBall = targetBall_;
		targetBall.GetComponent<NumberBall>().isTargetBall = true;
	}

	public void SetInaccuracy(float inaccuracy_)
	{
		inaccuracy = inaccuracy_;
	}

	private void DoCueVisual()
    {
		// set cue position to mouse
		Vector3 mousePos = Mouse.current.position.ReadValue();
		Vector3 mousePosCam = Camera.main.ScreenToWorldPoint(mousePos);
		mousePosCam.z = 0;
		transform.position = mousePosCam;

		// set distance to fixed max amount away from cue ball pos
		Vector3 cuePos = targetBall.transform.position;
		Vector3 v = mousePosCam - cuePos;

		if (v.magnitude > maxCueDistance)
		{
			transform.position = (v.normalized * maxCueDistance) + cuePos;
		}

		// rotate cue around cue ball
		float xDiff = cuePos.x - mousePosCam.x;
		float yDiff = cuePos.y - mousePosCam.y;

		if (xDiff < 0)
		{
			float f = (Mathf.Atan(yDiff / xDiff) * (180 / Mathf.PI)) + 180;
			Quaternion q = Quaternion.Euler(0, 0, f);
			transform.rotation = q;
		}
		else
		{
			float f = Mathf.Atan(yDiff / xDiff) * (180 / Mathf.PI);
			Quaternion q = Quaternion.Euler(0, 0, f);
			transform.rotation = q;
		}

		// set transparency
		// need to add if statement for ShopOpen or DoingClickPowerUp (vars don't exist yet
		if (clickedOnBall)
		{
			spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
		}
		else
		{
			spriteRenderer.color = new Color(1f, 1f, 1f, .3f);
		}

		if (powerUpHandler.IsAnyActive())
		{
			spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
		}
	}

	private void CheckAngerInaccuracy()
	{
		float rage = 0.0f;
		if (GameState.billiardsP1Turn)
		{
			rage = GameState.p1Rage;
		}
		else
		{
			rage = GameState.p2Rage;
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

	private void CheckCanShoot()
	{
		canShoot = true;
		if (GameState.isShopOpen || powerUpHandler.IsAnyActive() )
		{
			noShootTimer = 0.2f;
		}
		if (noShootTimer > 0)
		{
			canShoot = false;
		}
		noShootTimer = Mathf.Max(0.0f, noShootTimer - Time.deltaTime);
	}

	private void CheckClicking()
	{
		if (canShoot && clickAction.WasPressedThisFrame())
		{
			if (!hasHit || secondTapAvailable)
			{
				Vector2 vec = pointAction.ReadValue<Vector2>();
				Vector2 worldVec = Camera.main.ScreenToWorldPoint(vec);
				if (Vector2.Distance(worldVec, targetBall.transform.position) <= 1.0f)
				{
					clickedOnBall = true;
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
			lineRenderer.enabled = false;
		}
	}

	private void Aim()
	{
		if (clickedOnBall)
		{
			lineRenderer.enabled = seePath;
			lineRenderer.SetPosition(0, targetBall.transform.position);
			Vector2 vec = pointAction.ReadValue<Vector2>();
			Vector3 worldVec = Camera.main.ScreenToWorldPoint(vec);
			worldVec.z = 0;
			var ballPos = targetBall.transform.position;
			ballPos.z = 0;
			Vector3 endPos = (ballPos - worldVec).normalized * 10.0f;
			endPos = endPos + ballPos;
			lineRenderer.SetPosition(1, endPos);

			if (clickAction.WasReleasedThisFrame())
			{
				clickedOnBall = false;
				if (lineRenderer != null)
				{
					lineRenderer.enabled = false;
				}
				Shoot();
			}
		}
	}

	private void Shoot()
	{
		Rigidbody2D rb = targetBall.GetComponent<Rigidbody2D>();
		if (rb != null)
		{
			Vector2 vec = pointAction.ReadValue<Vector2>();
			Vector3 worldVec = Camera.main.ScreenToWorldPoint(vec);
			Vector2 distanceVec = targetBall.transform.position - worldVec;
			float distance = Mathf.Min(distanceVec.magnitude, MAX_POWER_DISTANCE);
			float randomAngle = (inaccuracy + angerInaccuracy) * Random.Range(-1, 1);
			float shotAngle = Mathf.Atan2(distanceVec.y, distanceVec.x);
			float x = Mathf.Cos(shotAngle + randomAngle);
			float y = Mathf.Sin(shotAngle + randomAngle);
			Vector2 f = new Vector2(x, y);
			f = f.normalized * distance * forceMult;
			rb.AddForce(f, ForceMode2D.Impulse);

			NumberBall t = targetBall.GetComponent<NumberBall>();
			if (!t.isCueBall && !t.is8Ball && ((GameState.billiardsP1Turn && GameState.billiardsP1Solids) != t.isStripe))
			{
				GameState.billiardsCorrectFirstCollision = true;
				GameState.billiardsGotFirstCollision = true;
			}

			if (!hasHit)
			{
				hasHit = true;
				if (!secondTapAvailable)
				{
					spriteRenderer.enabled = false;
				}
			}
			else
			{
				secondTapAvailable = false;
				lineRenderer.enabled = false;
				spriteRenderer.enabled = false;
			}
		}
	}
}
