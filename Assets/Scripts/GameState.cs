using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.InputSystem;

public class GameState : MonoBehaviour
{
    public enum States { TITLE, BILLIARDS, FIGHT, FIGHT_SCORING, GAME_OVER };

    public States state { get; private set; } = States.TITLE;

    public const float MINIMUM_FIGHT_RAGE = 7.0f;

	public int gameWinner = -1;
	public int fightWinner = -1;
	public float fightTimer = 0.0f;
	public bool isBilliardsP1Turn = true;
	public int p1BilliardsScore = 0;
    public int p2BilliardsScore = 0;
    public float p1Rage = 0.0f;
    public float p2Rage = 0.0f;
    public int p1FightScore = 0;
    public int p2FightScore = 0;
    public int p1SkillPoints = 0;
	public int p2SkillPoints = 0;
    public bool isShopOpen = false;

	private PlayerInput playerInput;
	private InputAction startBilliardsAction;
	private InputAction startFightAction;
	private InputAction scoreFightAction;
    private InputAction gameOverAction;

	public CameraHandler cameraHandler;
	//public ScoreManager scoreManager;
	//public RageManager rageManager;
    public GameObject cue;
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
        fightUI.SetActive(false);
		fightMusic.volume = 0.0f;
		billiardsMusic.volume = 0.5f;
        cameraHandler.SetTarget(new Vector3(0, 0, -10), 6.5f);
	}

    public void StartFight()
    {
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
}
