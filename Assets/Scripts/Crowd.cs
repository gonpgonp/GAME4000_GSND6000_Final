using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Crowd : MonoBehaviour
{

    public Animator CrowdLeft;
    public Animator CrowdRight;

    void Start()
    {
        CrowdLeft.Play("CrowdLeft_Calm");
        CrowdRight.Play("CrowdRight_calm");
    }

    public void UpdateCrowdAnims()
    {
        //CrowdLeft.Play("CrowdLeft_Calm");
        //CrowdRight.Play("CrowdRight_calm");
        
        int fightRatio = GetFightRatio();
        Debug.Log("FightRatio: " + fightRatio);

        if (fightRatio > 5)
        {
            CrowdLeft.Play("CrowdLeft_Wild");
        }
        else
        {
            CrowdLeft.Play("CrowdLeft_Calm");
        }
        if (fightRatio < 5)
        {
            CrowdRight.Play("CrowdRight_Wild");
        }
        else
        {
            CrowdRight.Play("CrowdRight_calm");
        }
    }

    public int GetFightRatio()
    {  
        int fightRatio = 5;
        float p1Score = 1;
        float p2Score = 1;

        if (GameState.p1FightScore != 0)
        {
            p1Score = GameState.p1FightScore;
        }

        if (GameState.p2FightScore != 0)
        {
            p2Score = GameState.p2FightScore;
        }

        float f = p1Score / (p1Score + p2Score);
        f *= 10;
        fightRatio = Mathf.RoundToInt(f);

        return fightRatio;
    }
}
