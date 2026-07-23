using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class SkillTreeUI : MonoBehaviour
{
    public Button xButton;
    public bool shopOpen;

    public PowerUpHandler powerUpHandler;
    public SkillUnlockManager skillUnlockManager;
    public SkillButton cue1;
    public SkillButton cue2;
    public SkillButton cue3;
    public SkillButton ball1;
    public SkillButton ball2;
    public SkillButton ball3;
    public SkillButton table1;
    public SkillButton table2;
    public SkillButton table3;


    public GameObject cue1CostObj;
    public GameObject cue2CostObj;
    public GameObject cue3CostObj;
    public GameObject ball1CostObj;
    public GameObject ball2CostObj;
    public GameObject ball3CostObj;
    public GameObject table1CostObj;
    public GameObject table2CostObj;
    public GameObject table3CostObj;

    public GameObject p1SwagDisplay;
    public GameObject p2SwagDisplay;
  



    void Start()
    {
        // add event listener for close
        xButton.onClick.AddListener(XButtonCloseShop);

        // set the swagger on loading the shop
        if (GameState.isBilliardsP1Turn && p1SwagDisplay.GetComponent<TMP_Text>() != null)
        {
            string swagStr = GameState.p1SkillPoints.ToString();
            p1SwagDisplay.GetComponent<TMP_Text>().SetText(swagStr);
        }
        else if (!GameState.isBilliardsP1Turn && p2SwagDisplay.GetComponent<TMP_Text>() != null)
        {
            string swagStr = GameState.p2SkillPoints.ToString();
            p2SwagDisplay.GetComponent<TMP_Text>().SetText(swagStr);
        }  
        
        // initialize arrays
        int[] skillCosts =
        {
            cue1.cost, cue2.cost, cue3.cost,
            ball1.cost, ball2.cost, ball3.cost,
            table1.cost, table2.cost, table3.cost
        };

        GameObject[] skillCostObjs = 
        {
            cue1CostObj, cue2CostObj, cue3CostObj, 
            ball1CostObj, ball2CostObj, ball3CostObj, 
            table1CostObj, table2CostObj, table3CostObj
        };

        // set all the costs
        for (int i=0; i<9; i++)
        {
            if (skillCostObjs[i].GetComponent<TMP_Text>() != null)
            {
                string str = skillCosts[i].ToString();
                skillCostObjs[i].GetComponent<TMP_Text>().SetText(str);
            }
        }     
    }

    void Update()
    {

    }

    public void UpdateAllButtons()
    {
        SkillButton[] skills =
        {
            cue1, cue2, cue3, 
            ball1, ball2, ball3, 
            table1, table2, table3
        };
        
        SkillButton btn;

        for (int i=0; i<9; i++)
        {
            btn = skills[i];
            btn.SetButtonState();
            btn.SetButtonBehavior();
        }

    }

    void XButtonCloseShop()
    {
        gameObject.SetActive(false);
        GameState.isShopOpen = false;
    }
}
