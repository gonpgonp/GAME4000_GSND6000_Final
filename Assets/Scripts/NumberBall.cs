using UnityEngine;

public class NumberBall : MonoBehaviour
{    
    public bool is8Ball;
    public bool isStripe;
    public ScoreManager scoreManager;
    public RageManager rageManager;
    public CueBall cueBall;
    public BilliardsUI billiardsUI;

    void Start()
    {
        
    }
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // number ball goes in pocket
        if (collider.CompareTag("Pocket"))
        {
            gameObject.SetActive(false);
        }
        // checking if 8ball was sunk
        if (!is8Ball)
        {
            if (isStripe)
            {
                scoreManager.p2Score += 1;
                billiardsUI.UpdateBilliardsScoreUI();
                rageManager.p1Rage += 1;
                billiardsUI.SetRageMeter();
            }
            else
            {
                scoreManager.p1Score += 1;
                billiardsUI.UpdateBilliardsScoreUI();
                rageManager.p2Rage += 1;
                billiardsUI.SetRageMeter();
            }
        }
        else
        {
            scoreManager.CheckWin();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) // still need to add sfx
    {  
        // number ball rage checkers
        if (cueBall.hasBroken)
        {
            if (collision.collider.CompareTag("NumberBall") || collision.collider.CompareTag("Player")) // if this number ball gets hit by any other ball
            {
                if (!scoreManager.billiardsIsP2Turn)
                {
                    if (!isStripe) // p1's turn, solid got hit
                    {
                        cueBall.cueHitMyBall = true;
                    }
                }
                else // p2's turn, hits a ball
                {
                    if (isStripe) // p2's turn, hits a solid
                    {
                        cueBall.cueHitMyBall = true;
                    }
                }
            }
        }
    }    
}
