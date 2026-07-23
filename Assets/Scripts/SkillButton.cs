using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SkillButton : MonoBehaviour
{
    public int level; // what tier this skill is (1, 2, 3)
    public int cost; // how much it costs
    public int state;  // 0 = avail, 1 = unavail, 2 = avail but can't afford, 3 = bought, 4 = usable state in hotbar, 5 = hotbar timeout

    public SkillUnlockManager skillUnlockManager;
    public Sprite availSprite;
    public Sprite cantAffordSprite;
    public Sprite unavailSprite;
    public Sprite boughtSprite;
    public GameObject swagDisplay;

    public GameObject priceBg;
    public GameObject priceObj;

    public SkillTreeUI skillTreeUI;

    public Hotbar hotbar;

    void Start()
    {
        SetButtonState();
        SetButtonBehavior();
    }

    public void SetButtonState()
    {
        // state 4 and 5 get set externally in hotbar code
        if (state == 4 || state == 5)
        {
            return;
        }

        int swag;

        if (GameState.isBilliardsP1Turn)
        {
            swag = GameState.p1SkillPoints;
        }
        else
        {
            swag = GameState.p2SkillPoints;
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
        else if (state == 4)
        {
            button.image.sprite = availSprite;
            priceBg.SetActive(false);
            priceObj.SetActive(false);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(UseSkillFromHotbar);
        }
        else if (state == 5)
        {
            priceBg.SetActive(false);
            priceObj.SetActive(false);
            button.image.sprite = unavailSprite;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(SkillTimeout);
        }
    }

    void BuySkill()
    {
        Debug.Log("you bought the skill");

        string str;

        // subtract the cost and update the swag score
        if (GameState.isBilliardsP1Turn)
        {
			GameState.p1SkillPoints -= cost;
            str = GameState.p1SkillPoints.ToString();
        }
        else
        {
			GameState.p2SkillPoints -= cost;
            str = GameState.p2SkillPoints.ToString();
        }

        swagDisplay.GetComponent<TMP_Text>().SetText(str);

        if (this.CompareTag("CueSkill"))
        {
            skillUnlockManager.cue++;
        }
        if (this.CompareTag("BallSkill"))
        {
            skillUnlockManager.ball++;
        }
        if (this.CompareTag("TableSkill"))
        {
            skillUnlockManager.table++;
        }

        // set this state to bought
        state = 3;

        // add bought skill to hotbar
        hotbar.hotbarSkills.Add(this);
        hotbar.OrganizeHotbarSkills();
        hotbar.DisplayHotbarSkills();

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

    void UseSkillFromHotbar()
    {
        // grab the skill's category from name, then inside poweruphandler do the correct ActivateXXXAbility(level-1)
        Debug.Log("you clicked on this skill in the hotbar");
        state = 5;
        SetButtonBehavior();
    }

    void SkillTimeout()
    {
        Debug.Log("you already used this skill");
    }
}
