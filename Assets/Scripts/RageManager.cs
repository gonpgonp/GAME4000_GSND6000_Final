using UnityEngine;

public class RageManager : MonoBehaviour
{
    public int p1Rage = 0;
    public int p2Rage = 0;

    public int minimumFightRage = 8;

    public ScoreManager scoreManager;
    public CueBall cueBall;
    public BilliardsUI billiardsUI;

    public void AddRageEndOfTurn()
    {
        if (!scoreManager.billiardsIsP2Turn)
        {
            if (!cueBall.cueHitAnyBall) // p1 didn't hit any number balls
            {
                p1Rage += 2;
                billiardsUI.SetRageMeter();
            }
            if (!cueBall.cueHitMyBall) // p1 didn't hit any of own balls
            {
                p1Rage += 2;
                billiardsUI.SetRageMeter();
            }
        }
        else
        {
            if (!cueBall.cueHitAnyBall) // p2 didn't hit any number balls
            {
                p2Rage += 2;
                billiardsUI.SetRageMeter();
            }
            if (!cueBall.cueHitMyBall) // p2 didn't hit any of own balls
            {
                p2Rage += 2;
                billiardsUI.SetRageMeter();
            }
        }

        // need to add rage if you don't score that turn, but may change in pool rules update, with scratching?

        cueBall.cueHitAnyBall = false;
        cueBall.cueHitMyBall = false;
    }

}
