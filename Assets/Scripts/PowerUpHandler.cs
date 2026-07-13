using System.Collections.Generic;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

public class PowerUpHandler : MonoBehaviour
{
	private PlayerInput playerInput;
	private InputAction clickAction;
	private InputAction pointAction;

	private InputAction cue1Action;
	private InputAction cue2Action;
	private InputAction cue3Action;
	private InputAction ball1Action;
	private InputAction ball2Action;
	private InputAction ball3Action;
	private InputAction table1Action;
	private InputAction table2Action;
	private InputAction table3Action;

	public Camera camera_;

    public GameObject cueBall;
    public GameObject pocketPreview;

    private GameObject swapBall = null;

    private bool[] cueActive = new bool[3];
    private bool[] ballActive = new bool[3];
    private bool[] tableActive = new bool[3];

    public GameObject P1Shop;
    public GameObject P2Shop;

    public GameObject PocketPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		playerInput = GetComponent<PlayerInput>();
		clickAction = playerInput.currentActionMap.FindAction("Click");
		pointAction = playerInput.currentActionMap.FindAction("Point");

        cue1Action = playerInput.currentActionMap.FindAction("Cue1");
        cue2Action = playerInput.currentActionMap.FindAction("Cue2");
		cue3Action = playerInput.currentActionMap.FindAction("Cue3");
        ball1Action = playerInput.currentActionMap.FindAction("Ball1");
		ball2Action = playerInput.currentActionMap.FindAction("Ball2");
		ball3Action = playerInput.currentActionMap.FindAction("Ball3");
        table1Action = playerInput.currentActionMap.FindAction("Table1");
		table2Action = playerInput.currentActionMap.FindAction("Table2");
		table3Action = playerInput.currentActionMap.FindAction("Table3");
	}

    // Update is called once per frame
    void Update()
    {
        if (!IsAnyActive())
        {
            if (cue1Action.WasPressedThisFrame())
            {
                cueActive[0] = true;
            }
            else if (cue2Action.WasPressedThisFrame())
            {
                cueActive[1] = true;
            }
            else if (cue3Action.WasPressedThisFrame())
            {
                cueActive[2] = true;
            }
            else if (ball1Action.WasPressedThisFrame())
            {
                ballActive[0] = true;
            }
            else if (ball2Action.WasPressedThisFrame())
            {
                ballActive[1] = true;
            }
            else if (ball3Action.WasPressedThisFrame())
            {
                ballActive[2] = true;
            }
            else if (table1Action.WasPressedThisFrame())
            {
                tableActive[0] = true;
            }
            else if (table2Action.WasPressedThisFrame())
            {
                tableActive[1] = true;
            }
            else if (table3Action.WasPressedThisFrame())
            {
                tableActive[2] = true;
            }
        }

        if (cueActive[0])
        {
            SeePath();
        }
        else if (cueActive[1])
        {
            SecondTap();
        }
        else if (cueActive[2])
        {
            InaccurateShot();
        }
        else if (ballActive[0])
        {
            SwapBalls();
        }
        else if (ballActive[1])
        {
            ShuffleBalls();
        }
        else if (ballActive[2])
        {
            HeavyBall();
        }
        else if (tableActive[0])
        {
            MagneticPocket();
        }
        else if (tableActive[1])
        {
            BlockPocket();
        }
        else if (tableActive[2])
        {
            AddPocket();
        }
    }

    public void ResetPowerUps()
    {
        for (int i = 0; i < 3; i++)
        {
            cueActive[i] = false;
            ballActive[i] = false;
            tableActive[i] = false;
        }

        CueBall cb = cueBall.GetComponent<CueBall>();
		cb.SetSeePath(true);
        cb.SetInaccuracy(0.0f);
        cb.SetSecondTap(false);
        cb.SetMass(1.0f);

		//reset shop activations (probably will get rewritten to hotbar activations that need to be reset
	}

    private void SeePath()
    {
		cueBall.GetComponent<CueBall>().SetSeePath(true);
		cueActive[0] = false;
    }

    private void SecondTap()
    {
        cueBall.GetComponent<CueBall>().SetSecondTap(true);
		cueActive[1] = false;
	}

    private void InaccurateShot()
    {
        cueBall.GetComponent<CueBall>().SetInaccuracy(10.0f);
		cueActive[2] = false;
	}

    private void SwapBalls()
    {
		Vector2 vec = pointAction.ReadValue<Vector2>();
		Vector3 worldVec = camera_.ScreenToWorldPoint(vec);
		var numBalls = GameObject.FindGameObjectsWithTag("NumberBall");
        GameObject hoveredBall = null;

		foreach (var ball in numBalls)
		{
			if (ball.GetComponent<Collider2D>().OverlapPoint(worldVec))
			{
				ball.GetComponent<SpriteRenderer>().color = Color.red;
                hoveredBall = ball;
			}
            else
            {
				ball.GetComponent<SpriteRenderer>().color = Color.white;
			}
		}

		if (clickAction.WasPressedThisFrame() && hoveredBall != null)
        {
            if (swapBall == null)
            {
                swapBall = hoveredBall;
            }
            else if (hoveredBall != swapBall)
            {
                Vector3 ball1Pos = swapBall.transform.position;
                swapBall.transform.position = hoveredBall.transform.position;
				hoveredBall.transform.position = ball1Pos;
				hoveredBall.GetComponent<SpriteRenderer>().color = Color.white;
				swapBall = null;
                ballActive[0] = false;
            }
		}
	}

    private void ShuffleBalls()
    {
		var numBalls = GameObject.FindGameObjectsWithTag("NumberBall");
        List<Vector3> positionList = new List<Vector3>();
		foreach (var ball in numBalls)
		{
            positionList.Add(ball.transform.position);
		}

		foreach (var ball in numBalls)
		{
            int i = Random.Range(0, positionList.Count);
			ball.transform.position = positionList[i];
            positionList.RemoveAt(i);
		}

		ballActive[1] = false;
	}

    private void HeavyBall()
    {
		cueBall.GetComponent<CueBall>().SetMass(10.0f);
		ballActive[2] = false;
	}

    private void MagneticPocket()
    {
		Vector2 vec = pointAction.ReadValue<Vector2>();
		Vector3 worldVec = camera_.ScreenToWorldPoint(vec);
		var pockets = GameObject.FindGameObjectsWithTag("Pocket");
		GameObject hoveredPocket = null;

		foreach (var pocket in pockets)
		{
			if (pocket.GetComponent<Collider2D>().OverlapPoint(worldVec))
			{
				pocket.GetComponent<SpriteRenderer>().color = Color.red;
				hoveredPocket = pocket;
			}
			else
			{
				pocket.GetComponent<SpriteRenderer>().color = Color.black;
			}
		}


		if (clickAction.WasPressedThisFrame() && hoveredPocket != null)
        {
            //give pocket gravity
            hoveredPocket.GetComponent<Pocket>().SetGravity(true);
			hoveredPocket.GetComponent<SpriteRenderer>().color = Color.black;
            tableActive[0] = false;
        }
    }

    private void BlockPocket()
    {
		Vector2 vec = pointAction.ReadValue<Vector2>();
		Vector3 worldVec = camera_.ScreenToWorldPoint(vec);
		var pockets = GameObject.FindGameObjectsWithTag("Pocket");
		GameObject hoveredPocket = null;

		foreach (var pocket in pockets)
		{
			if (pocket.GetComponent<Collider2D>().OverlapPoint(worldVec))
			{
				pocket.GetComponent<SpriteRenderer>().color = Color.red;
				hoveredPocket = pocket;
			}
			else
			{
				pocket.GetComponent<SpriteRenderer>().color = Color.black;
			}
		}

		if (clickAction.WasPressedThisFrame() && hoveredPocket != null)
		{
			//give pocket blocker
			hoveredPocket.GetComponent<Pocket>().SetBlocker(true);
			hoveredPocket.GetComponent<SpriteRenderer>().color = Color.black;
			tableActive[1] = false;
		}
	}

    private void AddPocket()
    {
		Vector2 vec = pointAction.ReadValue<Vector2>();
		Vector3 worldVec = camera_.ScreenToWorldPoint(vec);
        worldVec.x = Mathf.Clamp(worldVec.x, -8, 8);
        worldVec.y = Mathf.Clamp(worldVec.y, -4, 4);
        worldVec.z = 0f;
        pocketPreview.transform.position = worldVec;

		if (clickAction.WasPressedThisFrame())
		{
            Instantiate(PocketPrefab, worldVec, Quaternion.identity);
            pocketPreview.transform.position = new Vector3(100, 0, 0);
			tableActive[2] = false;
		}
	}

    public void ActivateCueAbility(int i)
    {
        cueActive[i] = true;
    }

    public void ActivateBallAbility(int i)
    {
        ballActive[i] = true;
    }

    public void ActivateTableAbility(int i)
    {
        tableActive[i] = true;
    }
    
    public bool IsAnyActive()
    {
        foreach (var c in cueActive)
        {
            if (c) return true;
        }

		foreach (var b in ballActive)
		{
			if (b) return true;
		}

		foreach (var t in tableActive)
		{
			if (t) return true;
		}

        return false;
	}
}
