using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public int p1Score = 0;
    public int p2Score = 0;

    public int p1Swag = 0;
    public int p2Swag = 0;

    public bool billiardsIsP2Turn = false;
    public int winner;
    public GameObject cueBall;
    bool allBallsStopped;

    public RageManager rageManager;

    public BilliardsUI billiardsUI;

    void Update()
    {
        if (CheckAllBallsStopped())
        {
            ChangeTurns();
            allBallsStopped = false;
        }        
    }

    public bool CheckAllBallsStopped()
    {
        if (cueBall.GetComponent<CueBall>() != null)
        {
            if (cueBall.GetComponent<CueBall>().hasHit == true && !cueBall.GetComponent<CueBall>().secondTapAvailable)
            {
                allBallsStopped = true;

                GameObject[] numBalls = GameObject.FindGameObjectsWithTag("NumberBall");

                Rigidbody2D cueRb = cueBall.GetComponent<Rigidbody2D>();
                float cueSpeed = cueRb.linearVelocity.magnitude;

                if (cueSpeed > 0.1f) // if the cue ball is still moving, allBallsStopped = false;
                {
                    allBallsStopped = false;
                }
                else // if the cue ball is still, check if any numballs are still moving; if so, set allBallsStopped to false
                {
                    foreach (GameObject numBall in numBalls)
                    {
                        Rigidbody2D numRb = numBall.GetComponent<Rigidbody2D>();

                        if (numRb.linearVelocity.magnitude > 0.1f)
                        {
                            allBallsStopped = false;
                        }
                    }
                }

            }
        }
        return allBallsStopped;
    }

    public void ChangeTurns()
    {
        rageManager.AddRageEndOfTurn();
        billiardsIsP2Turn = !billiardsIsP2Turn; // swap the active player
        Debug.Log("turn change");
        billiardsUI.SetTurnUI();

        // anything else that would need to happen on a turn change; UI is handled in BilliardsUI.SetTurnUI()
    }

    public void CheckWin() // only gets called when 8ball is sunk in NumberBall script
    {
        if (!billiardsIsP2Turn)
        {
            if (p1Score >= 7) // p1 is the winner
            {
                winner = 1;
            }
            else // p1 sunk the 8ball early
            {
                winner = 2;
            }

        }
        else
        {
            if (p2Score >= 7) // p2 is the winner
            {
                winner = 2;
            }
            else // p2 sunk the 8ball early
            {
                winner = 1;
            }
        }
    }
}
