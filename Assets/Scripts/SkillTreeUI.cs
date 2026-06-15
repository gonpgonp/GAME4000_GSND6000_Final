using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
public class SkillTreeUI : MonoBehaviour
{
    public Button xButton;
    public bool shopOpen;

    PowerUpHandler powerUpHandler;
    
    int cue1Cost = 1;
    int cue2Cost = 2;
    int cue3Cost = 3;
    int ball1Cost = 1;
    int ball2Cost = 2;
    int ball3Cost = 3;
    int table1Cost = 1;
    int table2Cost = 2;
    int table3Cost = 3;

    public GameObject cue1CostObj;
    public GameObject cue2CostObj;
    public GameObject cue3CostObj;
    public GameObject ball1CostObj;
    public GameObject ball2CostObj;
    public GameObject ball3CostObj;
    public GameObject table1CostObj;
    public GameObject table2CostObj;
    public GameObject table3CostObj;

    void Start()
    {
        xButton.onClick.AddListener(XButtonCloseShop);

        int[] skillCosts = {cue1Cost, cue2Cost, cue3Cost, ball1Cost, ball2Cost, ball3Cost, table1Cost, table2Cost, table3Cost};
        GameObject[] skillCostObjs = {cue1CostObj, cue2CostObj, cue3CostObj, ball1CostObj, ball2CostObj, ball3CostObj, table1CostObj, table2CostObj, table3CostObj};

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

    void XButtonCloseShop()
    {
        gameObject.SetActive(false);  
    }


}
