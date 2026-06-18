using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class SkillTreeUI : MonoBehaviour
{
    public Button xButton;
    public bool shopOpen;

    public PowerUpHandler powerUpHandler;
    public ScoreManager scoreManager;
    public SkillUnlockManager skillUnlockManager;


    public GameObject cue1CostObj;
    public GameObject cue2CostObj;
    public GameObject cue3CostObj;
    public GameObject ball1CostObj;
    public GameObject ball2CostObj;
    public GameObject ball3CostObj;
    public GameObject table1CostObj;
    public GameObject table2CostObj;
    public GameObject table3CostObj;

    public Button cue1Btn;
    public Button cue2Btn;
    public Button cue3Btn;
    public Button ball1Btn;
    public Button ball2Btn;
    public Button ball3Btn;
    public Button table1Btn;
    public Button table2Btn;
    public Button table3Btn;

    public GameObject p1SwagDisplay;
    public GameObject p2SwagDisplay;

    void Start()
    {
        // add event listener for close
        xButton.onClick.AddListener(XButtonCloseShop);

        int[] skillCosts = 
        {skillUnlockManager.cue1Cost, skillUnlockManager.cue2Cost, skillUnlockManager.cue3Cost, 
        skillUnlockManager.ball1Cost, skillUnlockManager.ball2Cost, skillUnlockManager.ball3Cost, 
        skillUnlockManager.table1Cost, skillUnlockManager.table2Cost, skillUnlockManager.table3Cost};

        GameObject[] skillCostObjs = 
        {cue1CostObj, cue2CostObj, cue3CostObj, 
        ball1CostObj, ball2CostObj, ball3CostObj, 
        table1CostObj, table2CostObj, table3CostObj};

        Button[] skillBtns = 
        {cue1Btn, cue2Btn, cue3Btn,
        ball1Btn, ball2Btn, ball3Btn,
        table1Btn, table2Btn, table3Btn};


        // set all the costs
        for (int i=0; i<9; i++)
        {
            if (skillCostObjs[i].GetComponent<TMP_Text>() != null)
            {
                string str = skillCosts[i].ToString();
                skillCostObjs[i].GetComponent<TMP_Text>().SetText(str);
            }
        }

        // set the swagger on loading the shop
        if (!scoreManager.billiardsIsP2Turn && p1SwagDisplay.GetComponent<TMP_Text>() != null)
        {
            string swagStr = scoreManager.p1Swag.ToString();
            p1SwagDisplay.GetComponent<TMP_Text>().SetText(swagStr);
        }
        else if (scoreManager.billiardsIsP2Turn && p2SwagDisplay.GetComponent<TMP_Text>() != null)
        {
            string swagStr = scoreManager.p2Swag.ToString();
            p2SwagDisplay.GetComponent<TMP_Text>().SetText(swagStr);
        }

        // check availability of skills
        bool[] skillAvailability = skillUnlockManager.GetSkillAvailability();
        bool[] canAfford = skillUnlockManager.GetCanAffordSkills();
        
        for (int i=0; i<9; i++)
        {
            if (skillAvailability[i] && canAfford[i])
            {
                skillBtns[i].onClick.AddListener(BuySkill);
            }
            else if (skillAvailability[i])
            {
                skillBtns[i].onClick.AddListener(CantAfford);
            }
            else
            {
                skillBtns[i].onClick.AddListener(Unavail);
            }
        }

    }

    void Update()
    {

    }

    void BuySkill()
    {
        Debug.Log("skill buyable yay");
    }

    void CantAfford()
    {
        Debug.Log("this is avail but you can't afford it");
    }

    void Unavail()
    {
        Debug.Log("this isn't available");
    }

    void XButtonCloseShop()
    {
        gameObject.SetActive(false);  
    }

}
