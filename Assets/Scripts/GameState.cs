using UnityEngine;
using System.Collections;
using UnityEngine.Rendering.Universal;
using TMPro;
using UnityEngine.InputSystem;

public class GameState : MonoBehaviour
{
    int gameState = 0; //0 = title, 1 = billiards, 2 = fight, 3 = fight score counting, 10 = game over
	int gameWinner = -1;
	int fightWinner = -1;
	float fightTimer = 0.0f;

    int p1BilliardsScore = 0;
    int p2BilliardsScore = 0;
    float p1Rage = 0.0f;
    float p2Rage = 0.0f;
    int p1FightScore = 0;
    int p2FightScore = 0;
    int p1SkillPoints = 0;
    int p2SkillPoints = 0;

	private PlayerInput playerInput;
	private InputAction startBilliardsAction;
	private InputAction startFightAction;
	private InputAction scoreFightAction;

	public CameraHandler cameraHandler;
	//public ScoreManager scoreManager;
	//public RageManager rageManager;
    public GameObject cue;
    public GameObject billiardsUI;
    public GameObject fightUI;
    public GameObject player1;
    public GameObject player2;
    public GameObject scoreOverlay;
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
		StartBilliards();
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
    }

    void StartBilliards()
    {
        gameState = 1;
        cue.SetActive(true);
        billiardsUI.SetActive(true);
        fightUI.SetActive(false);
		fightMusic.volume = 0.0f;
		billiardsMusic.volume = 0.5f;
        cameraHandler.SetTarget(new Vector3(0, 0, -10), 6.5f);
	}

    void StartFight()
    {
		gameState = 2;
		cue.SetActive(true);
		billiardsUI.SetActive(true);
		fightUI.SetActive(false);
		fightMusic.volume = 0.2f;
		billiardsMusic.volume = 0.0f;
		cameraHandler.SetTarget(new Vector3(0, -10, -10), 17.0f);
	}

    void GameOver()
    {
		gameState = 10;
		cue.SetActive(false);
        billiardsUI.SetActive(false);
        fightUI.SetActive(false);
	}

    IEnumerator ScoreFight()
    {
        gameState = 3;
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
        for (int i = 0; i < 5; i++)
        {
            if (p1FightScore > 0)
            {
                p1FightScore -= 1;
                p1SkillPoints += 1;
                dickHeadAnimator.Play("Count");
            }

			if (p2FightScore > 0)
			{
				p2FightScore -= 1;
				p2SkillPoints += 1;
				richardHeadAnimator.Play("Count");
			}

            p1ScoreText.SetText(p1FightScore.ToString());
            p2ScoreText.SetText(p2FightScore.ToString());

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
