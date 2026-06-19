using UnityEngine;

public class SkillUnlockManager : MonoBehaviour
{
    public int cue = 0;
    public int ball = 1;
    public int table = 2;

    public int cue1Cost = 1;
    public int cue2Cost = 2;
    public int cue3Cost = 3;
    public int ball1Cost = 1;
    public int ball2Cost = 2;
    public int ball3Cost = 3;
    public int table1Cost = 1;
    public int table2Cost = 2;
    public int table3Cost = 3;

    public ScoreManager scoreManager;
    void Start()
    {
        
    }
    void Update()
    {
        
    }

    public bool[] GetSkillAvailability()
    {
        bool[] skillAvailState = new bool[9];

        for (int i=0; i<=cue; i++)
        {
            skillAvailState[i] = true;
        }
        for (int i=0; i<=ball; i++)
        {
            skillAvailState[i+3] = true;
        }
        for (int i=0; i<=table; i++)
        {
            skillAvailState[i+6] = true;
        }
        
       /*for (int i=0; i<9; i++)
        {
            Debug.Log(skillAvailState[i]);
        }*/

        return skillAvailState;
    }

    public bool[] GetCanAffordSkills()
    {
        bool[] canAffordState = new bool[9];

        int[] costs = {cue1Cost, cue2Cost, cue3Cost, ball1Cost, ball2Cost, ball3Cost, table1Cost, table2Cost, table3Cost};

        for (int i=0; i<9; i++)
        {
            if (!scoreManager.billiardsIsP2Turn)
            {
                if (costs[i] <= scoreManager.p1Swag)
                {
                    canAffordState[i] = true;
                }
            }
            else
            {
                if (costs[i] <= scoreManager.p2Swag)
                {
                    canAffordState[i] = true;
                }
            }
        }

        /*for (int i=0; i<9; i++)
        {
           Debug.Log(canAffordState[i]);
        }*/

        return canAffordState;
    }
}
