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

    public GameObject p1SwagDisplay;
    public GameObject p2SwagDisplay;
  



    void Start()
    {
        // initialize arrays
        int[] skillCosts = 
        {skillUnlockManager.cue1Cost, skillUnlockManager.cue2Cost, skillUnlockManager.cue3Cost, 
        skillUnlockManager.ball1Cost, skillUnlockManager.ball2Cost, skillUnlockManager.ball3Cost, 
        skillUnlockManager.table1Cost, skillUnlockManager.table2Cost, skillUnlockManager.table3Cost};

        GameObject[] skillCostObjs = 
        {cue1CostObj, cue2CostObj, cue3CostObj, 
        ball1CostObj, ball2CostObj, ball3CostObj, 
        table1CostObj, table2CostObj, table3CostObj};

        // add event listener for close
        xButton.onClick.AddListener(XButtonCloseShop);

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
    }

    void Update()
    {

    }

    /*void BuySkill(Button btn, int cost, GameObject p1Swag, GameObject p2Swag)
    {
        Debug.Log("skill bought yay");

        // change the sprite to bought
        btn.GetComponent<Image>().sprite = boughtSprite;

        // subtract the swag needed to buy it & update the swag display
        if (!scoreManager.billiardsIsP2Turn)
        {
            scoreManager.p1Swag -= cost;
            string str = scoreManager.p1Swag.ToString();
            p1Swag.GetComponent<TMP_Text>().SetText(str);
        }
        else
        {
            scoreManager.p2Swag -= cost;
            string str = scoreManager.p2Swag.ToString();
            p2Swag.GetComponent<TMP_Text>().SetText(str);
        }
    }*/

    void XButtonCloseShop()
    {
        gameObject.SetActive(false);
    }
}
