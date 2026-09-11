using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class BilliardsUI : MonoBehaviour
{
    public ScoreManager scoreManager;
    public RageManager rageManager;
    public Image p1RageBar;
    public Image p2RageBar;
    public GameObject startFightButton;

    private Color32 rageColor = new Color32(255, 44, 0, 255);
    private Color32 calmColor = new Color32(255, 255, 255, 255);

    public TextMeshProUGUI p1ScoreTMP;
    public TextMeshProUGUI p2ScoreTMP;

    public GameObject dicksTurnObj;
    public GameObject richardsTurnObj;

    public GameObject dicksHead;
    public GameObject richardsHead;

    public Animator _dickHead;
    public Animator _richardHead;
    public Animator _pissedMessages;

    public Animator _p1TurnSwap;

    public Animator _p2TurnSwap;

    public GameObject p1Hotbar;
    public GameObject p2Hotbar;

    void Start()
    {
        SetRageMeter();
        SetTurnUI();
    }

    public void SetRageMeter()
    {
        float p1Rage = GameState.p1Rage;
        float p2Rage = GameState.p2Rage;

        p1RageBar.rectTransform.localScale = new Vector3(.4f, p1Rage/25, .4f);
        p2RageBar.rectTransform.localScale = new Vector3(.4f, p2Rage/25, .4f);
        // want to fine-tune this behavior to look a little nicer later

        if (p1Rage >= GameState.MINIMUM_FIGHT_RAGE)
        {
            p1RageBar.color = rageColor;
			if (_dickHead.isActiveAndEnabled)
				_dickHead.Play("DickHeadPissed");
        }
        else
        {
            p1RageBar.color = calmColor;
			if (_dickHead.isActiveAndEnabled)
				_dickHead.Play("DickCalm");
        }
    
    
        if (p2Rage >= GameState.MINIMUM_FIGHT_RAGE)
        {
            p2RageBar.color = rageColor;
			if (_richardHead.isActiveAndEnabled)
				_richardHead.Play("RichardHeadPissed");
        }
        else
        {
            p2RageBar.color = calmColor;
            if (_richardHead.isActiveAndEnabled)
				_richardHead.Play("RichardCalm");
        }
        
    }

    public void SetFightButton()
    {
        if (GameState.billiardsP1Turn && GameState.p1Rage >= GameState.MINIMUM_FIGHT_RAGE)
        {
			startFightButton.SetActive(true);
		}
        else if (!GameState.billiardsP1Turn && GameState.p2Rage >= GameState.MINIMUM_FIGHT_RAGE)
        {
			startFightButton.SetActive(true);
		}
        else
        {
			startFightButton.SetActive(false);
		}
	}

    public void UpdateBilliardsScoreUI()
    {
		p1ScoreTMP.text = GameState.p1BilliardsScore.ToString();
		p2ScoreTMP.text = GameState.p2BilliardsScore.ToString();
	}

    public void SetTurnUI()
    {
        if (GameState.billiardsP1Turn)
        {
            _p1TurnSwap.Play("P1Turn");
            
            dicksTurnObj.SetActive(true);
            dicksHead.SetActive(true);
            SetRageMeter();
            p1Hotbar.SetActive(true);

    
            richardsTurnObj.SetActive(false);
            richardsHead.SetActive(false);
            p2Hotbar.SetActive(false);

        }
        else
        {
            _p2TurnSwap.Play("P2Turn");
            
            richardsTurnObj.SetActive(true);
            richardsHead.SetActive(true);
            SetRageMeter();
            p2Hotbar.SetActive(true);

            dicksTurnObj.SetActive(false);
            dicksHead.SetActive(false);
            p1Hotbar.SetActive(false);
        }
    }

    public void SetPissedMessages()
    {
        if (GameState.billiardsP1Turn && GameState.p1Rage >= GameState.MINIMUM_FIGHT_RAGE)
        {
            _pissedMessages.Play("DickPissed");
        }
        else if (!GameState.billiardsP1Turn && GameState.p2Rage >= GameState.MINIMUM_FIGHT_RAGE)
        {
            _pissedMessages.Play("RichardPissed");
        }
        else
        {
            _pissedMessages.Play("NoOnePissed");
        }
    }
}
