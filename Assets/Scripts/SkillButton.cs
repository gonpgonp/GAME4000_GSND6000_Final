using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillButton : MonoBehaviour
{
    public int level; // what tier this skill is (1, 2, 3)
    public int cost; // how much it costs
    int state;  // 0 = avail, 1 = unavail, 2 = avail but can't afford, 3 = bought


    public ScoreManager scoreManager;
    public SkillUnlockManager skillUnlockManager;
    public Sprite availSprite;
    public Sprite cantAffordSprite;
    public Sprite unavailSprite;
    public Sprite boughtSprite;
    public GameObject swagDisplay;

    public GameObject priceBg;
    public GameObject priceObj;

    public SkillTreeUI skillTreeUI;

    void Start()
    {
        SetButtonState();
        SetButtonBehavior();
    }

    public void SetButtonState()
    {
        int swag;

        if (!scoreManager.billiardsIsP2Turn)
        {
            swag = scoreManager.p1Swag;
        }
        else
        {
            swag = scoreManager.p2Swag;
        }

        // set button's state based on a) if it's already bought and b) the skillUnlockManager state
        if (this.CompareTag("CueSkill"))
        {
            if (state == 3)
            {
                state = 3;
            }
            else if (skillUnlockManager.cue == level-1)
            {
                if (cost <= swag)
                {
                    state = 0; // has enough to afford it
                }
                else
                {
                    state = 2; // it's available, but can't afford it
                }
            }
            else
            {
                state = 1; // not avail
            }
        }
        if (this.CompareTag("BallSkill"))
        {
            if (state == 3)
            {
                state = 3;
            }
            else if (skillUnlockManager.ball == level-1)
            {
                if (cost <= swag)
                {
                    state = 0; // has enough to afford it
                }
                else
                {
                    state = 2; // it's available, but can't afford it
                }
            }
            else
            {
                state = 1; // not avail
            }
        }
        if (this.CompareTag("TableSkill"))
        {
            if (state == 3)
            {
                state = 3;
            }
            else if (skillUnlockManager.table == level-1)
            {
                if (cost <= swag)
                {
                    state = 0; // has enough to afford it
                }
                else
                {
                    state = 2; // it's available, but can't afford it
                }
            }
            else
            {
                state = 1; // not avail
            }
        }
    }

    public void SetButtonBehavior()
    {
        Button button = this.GetComponent<Button>();

        if (state == 0)
        {
            button.image.sprite = availSprite;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(BuySkill);
        }
        else if (state == 1)
        {
            button.image.sprite = unavailSprite;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Unavail);
        }
        else if (state == 2)
        {
            button.image.sprite = cantAffordSprite;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(CantAfford);
        }
        else if (state == 3)
        {
            button.image.sprite = boughtSprite;
            priceBg.SetActive(false);
            priceObj.SetActive(false);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Bought);
        }
    }

    void BuySkill()
    {
        Debug.Log("you bought the skill");

        string str;

        // subtract the cost and update the swag score
        if (!scoreManager.billiardsIsP2Turn)
        {
            scoreManager.p1Swag -= cost;
            str = scoreManager.p1Swag.ToString();
        }
        else
        {
            scoreManager.p2Swag -= cost;
            str = scoreManager.p2Swag.ToString();
        }

        swagDisplay.GetComponent<TMP_Text>().SetText(str);

        if (this.CompareTag("CueSkill"))
        {
            skillUnlockManager.cue++;
            Debug.Log("skillunlockmanager.cue is now" + skillUnlockManager.cue.ToString());

        }
        if (this.CompareTag("BallSkill"))
        {
            skillUnlockManager.ball++;
            Debug.Log("skillunlockmanager.ball is now" + skillUnlockManager.ball.ToString());

        }
        if (this.CompareTag("TableSkill"))
        {
            skillUnlockManager.table++;
            Debug.Log("skillunlockmanager.table is now" + skillUnlockManager.table.ToString());
        }

        // set this state to bought
        state = 3;

        // update everything based on new info
        skillTreeUI.UpdateAllButtons();
        
    }

    void Unavail()
    {
        Debug.Log("this isn't available");
    }

    void CantAfford()
    {
        Debug.Log("this is avail but you can't afford it");
    }

    void Bought()
    {
        Debug.Log("you already bought this");
    }
}
