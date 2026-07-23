using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.InputSystem;

public class GameState : MonoBehaviour
{
    public enum States { TITLE, BILLIARDS, FIGHT, FIGHT_SCORING, GAME_OVER };

    public static States state { get; private set; } = States.TITLE;

    public const float MINIMUM_FIGHT_RAGE = 7.0f;

	public static int gameWinner = -1;
	public static int fightWinner = -1;
	public static float fightTimer = 0.0f;
	public static bool isBilliardsP1Turn = true;
	public static bool billiardsScoredThisTurn = false;
	public static int p1BilliardsScore = 0;
    public static int p2BilliardsScore = 0;
    public static float p1Rage = 0.0f;
    public static float p2Rage = 0.0f;
    public static int p1FightScore = 0;
    public static int p2FightScore = 0;
    public static int p1SkillPoints = 0;
	public static int p2SkillPoints = 0;
    public static bool isShopOpen = false;

	private PlayerInput playerInput;
	private InputAction startBilliardsAction;
	private InputAction startFightAction;
	private InputAction scoreFightAction;
    private InputAction gameOverAction;

	public CameraHandler cameraHandler;
    public GameObject cue;
    public GameObject cueBall;
    public GameObject billiardsUI;
    public GameObject fightUI;
    public Fighter player1;
    public Fighter player2;
    public GameObject scoreOverlay;
    public GameObject winOverlay;
    public Animator dickHeadAnimator;
    public Animator richardHeadAnimator;
    public TextMeshProUGUI p1ScoreText;
    public TextMeshProUGUI p2ScoreText;
    public AudioSource billiardsMusic;
    public AudioSource fightMusic;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
		state= States.TITLE;

		gameWinner = -1;
		fightWinner = -1;
		fightTimer = 0.0f;
		isBilliardsP1Turn = true;
		billiardsScoredThisTurn = false;
		p1BilliardsScore = 0;
		p2BilliardsScore = 0;
		p1Rage = 0.0f;
		p2Rage = 0.0f;
		p1FightScore = 0;
		p2FightScore = 0;
		p1SkillPoints = 0;
		p2SkillPoints = 0;
		isShopOpen = false;

		playerInput = GetComponent<PlayerInput>();
        startBilliardsAction = playerInput.currentActionMap.FindAction("StartBilliards");
		startFightAction = playerInput.currentActionMap.FindAction("StartFight");
		scoreFightAction = playerInput.currentActionMap.FindAction("ScoreFight");
        gameOverAction = playerInput.currentActionMap.FindAction("GameOver");
        cue.SetActive(false);
        billiardsUI.SetActive(false);
        fightUI.SetActive(false);
		fightMusic.volume = 0.0f;
		billiardsMusic.volume = 0.5f;
	}

    // Update is called once per frame
    void Update()
    {
        if (gameWinner != -1)
        {
			GameOver();
		}

        if (startBilliardsAction.WasPressedThisFrame())
        {
            StartBilliards();
        }

        if (startFightAction.WasPressedThisFrame())
        {
            StartFight();
        }

        if (scoreFightAction.WasPressedThisFrame())
        {
            StartCoroutine(ScoreFight());
		}

        if (gameOverAction.WasPerformedThisFrame())
        {
            GameOver();
        }

        switch(state)
        {
			case States.BILLIARDS:
				CheckEndBilliardsTurn();
				break;
            case States.FIGHT:
                if (fightTimer > 0.0f)
                {
                    fightTimer -= Time.deltaTime;
                }
                else
                {
                    fightTimer = 0.0f;
                    StartCoroutine(ScoreFight());
                }
                break;
        }
    }

    public void StartBilliards()
    {
        player1.DoReset();
        player2.DoReset();
		state = States.BILLIARDS;
        cue.SetActive(true);
        billiardsUI.SetActive(true);
		BilliardsUI BUI = billiardsUI.GetComponent<BilliardsUI>();
		BUI.SetRageMeter();
		BUI.SetFightButton();
		BUI.SetTurnUI();
		BUI.SetPissedMessages();
		fightUI.SetActive(false);
		fightMusic.volume = 0.0f;
		billiardsMusic.volume = 0.5f;
        cameraHandler.SetTarget(new Vector3(0, 0, -10), 6.5f);
	}

    public void StartFight()
    {
		if (isBilliardsP1Turn)
		{
			p1Rage = 0.0f;
		}
		else
		{
			p2Rage = 0.0f;
		}
		state = States.FIGHT;
		cue.SetActive(false);
		billiardsUI.SetActive(false);
		fightUI.SetActive(true);
		fightMusic.volume = 0.2f;
		billiardsMusic.volume = 0.0f;
		cameraHandler.SetTarget(new Vector3(0, -10, -10), 17.0f);
        fightTimer = 10.0f;
	}

    public void GameOver()
    {
		state = States.GAME_OVER;
		cue.SetActive(false);
        billiardsUI.SetActive(false);
        fightUI.SetActive(false);
        winOverlay.SetActive(true);
	}

    IEnumerator ScoreFight()
    {
		state = States.FIGHT_SCORING;
        scoreOverlay.SetActive(true);
        richardHeadAnimator.Play("Idle");
        dickHeadAnimator.Play("Idle");
        if (p1FightScore > p2FightScore)
        {
            fightWinner = 1;
        }
        else if (p1FightScore < p2FightScore)
        {
            fightWinner = 2;
        }
        else
        {
            fightWinner = -1;
        }
		yield return new WaitForSeconds(1.0f);
        int p1ScoreCount = 0;
        int p2ScoreCount = 0;
        for (int i = 0; i < 5; i++)
        {
            if (p1FightScore > 0)
            {
                p1FightScore -= 1;
                p1SkillPoints += 1;
                p1ScoreCount += 1;
				dickHeadAnimator.Play("Count");
            }

			if (p2FightScore > 0)
			{
				p2FightScore -= 1;
				p2SkillPoints += 1;
                p2ScoreCount += 1;
				richardHeadAnimator.Play("Count");
			}

            p1ScoreText.SetText(p1ScoreCount.ToString());
            p2ScoreText.SetText(p2ScoreCount.ToString());

            yield return new WaitForSeconds(0.2f);
		}

		yield return new WaitForSeconds(0.3f);

        if (fightWinner == 1)
        {
			richardHeadAnimator.Play("Angry");
        }
        else if (fightWinner == 2)
        {
            dickHeadAnimator.Play("Angry");
        }

		yield return new WaitForSeconds(1.0f);

        scoreOverlay.SetActive(false);

        StartBilliards();

        yield return null;
    }

    public bool CheckBilliardsWinner()
    {
		bool gameOver = false;
		if (isBilliardsP1Turn)
		{
			if (p1BilliardsScore >= 7) // p1 is the winner
			{
				gameWinner = 1;
				winOverlay.SetActive(true);
				gameOver = true;
			}
			else // p1 sunk the 8ball early
			{
				gameWinner = 2;
				gameOver = true;
				winOverlay.SetActive(true);
			}

		}
		else
		{
			if (p2BilliardsScore >= 7) // p2 is the winner
			{
				gameWinner = 2;
				gameOver = true;
				winOverlay.SetActive(true);
			}
			else // p2 sunk the 8ball early
			{
				gameWinner = 1;
				gameOver = true;
				winOverlay.SetActive(true);
			}
		}
		if (gameOver)
		{
			winOverlay.SetActive(true);
			state = States.GAME_OVER;
			return true;
		}
		return false;
	}

	public void AddRageEndOfTurn()
	{
		CueBall CB = cueBall.GetComponent<CueBall>();
		BilliardsUI BUI = billiardsUI.GetComponent<BilliardsUI>();
		if (isBilliardsP1Turn)
		{
			if (!CB.cueHitAnyBall) // p1 didn't hit any number balls
			{
				p1Rage += 2;
			}
			if (!CB.cueHitMyBall) // p1 didn't hit any of own balls
			{
				p1Rage += 2;
				
			}
			if (!billiardsScoredThisTurn)
			{
				p1Rage += 2;
			}
		}
		else
		{
			if (!CB.cueHitAnyBall) // p2 didn't hit any number balls
			{
				p2Rage += 2;
			}
			if (!CB.cueHitMyBall) // p2 didn't hit any of own balls
			{
				p2Rage += 2;
			}
			if (!billiardsScoredThisTurn)
			{
				p2Rage += 2;
			}
		}
		

		// need to add rage if you don't score that turn, but may change in pool rules update, with scratching?

		CB.cueHitAnyBall = false;
		CB.cueHitMyBall = false;
	}

	public void ChangeBilliardsTurn()
    {
		BilliardsUI BUI = billiardsUI.GetComponent<BilliardsUI>();
		AddRageEndOfTurn();
		BUI.SetRageMeter();
		isBilliardsP1Turn = !isBilliardsP1Turn;
		BUI.SetFightButton();
		BUI.SetTurnUI();
		BUI.SetPissedMessages();
		billiardsScoredThisTurn = false;
	}

    public void CheckEndBilliardsTurn()
    {
		CueBall CB = cueBall.GetComponent<CueBall>();
		if (CB != null)
		{
			if (CB.hasHit == true && !CB.secondTapAvailable)
			{
				bool allBallsStopped = true;

				GameObject[] numBalls = GameObject.FindGameObjectsWithTag("NumberBall");

				Rigidbody2D cueRb = cueBall.GetComponent<Rigidbody2D>();
				float cueSpeed = cueRb.linearVelocity.magnitude;

				if (cueSpeed > 0.1f) // if the cue ball is still moving, allBallsStopped = false;
				{
					allBallsStopped = false;
				}
				else // if the cue ball is still, check if any numballs are still moving; if so, set allBallsStopped to false
				{
					foreach (GameObject numBall in numBalls)
					{
						Rigidbody2D numRb = numBall.GetComponent<Rigidbody2D>();

						if (numRb.linearVelocity.magnitude > 0.1f)
						{
							allBallsStopped = false;
						}
					}
				}

				if (allBallsStopped)
				{
					CB.ReadyForNextTurn();
					ChangeBilliardsTurn();
				}

			}
		}
	}
}
