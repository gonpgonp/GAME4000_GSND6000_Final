using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class CueBall : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction clickAction;
    private InputAction pointAction;

    Camera camera;
    GameObject cue;
	GameObject powerUpHandler;
	bool secondTapAvailable;
    float inaccuracy;
    float angerInaccuracy;

    float forceMult;
    bool clickedOnBall;
    bool hasHit;
    bool allBallsStopped;
    bool didScratch;
    float comparisonScore;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        clickAction = playerInput.currentActionMap.FindAction("Click");
        pointAction = playerInput.currentActionMap.FindAction("Point");
    }

    // Update is called once per frame
    void Update()
    {
		CheckAngerInaccuracy();
		CheckClicking();
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
        if (!Variables.Scene(gameObject.scene).Get<bool>("ShopOpen") && Variables.Scene(gameObject.scene).Get<bool>("isFight") && clickAction.WasPressedThisFrame())
        {
            if (!hasHit || secondTapAvailable)
            {
                Vector2 vec = pointAction.ReadValue<Vector2>();
                Vector2 worldVec = camera.ScreenToWorldPoint(vec);
                if (Vector2.Distance(worldVec, this.transform.position) <= 0.5)
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
    }

    private void Aim()
    {
        if (clickedOnBall)
        {
            if (clickAction.IsPressed())
            {

            }
        }
    }
}
