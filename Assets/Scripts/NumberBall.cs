using UnityEngine;

public class NumberBall : MonoBehaviour
{    
    public bool is8Ball;
    public bool isStripe;
    public ScoreManager scoreManager;
    public RageManager rageManager;
    public CueBall cueBall;
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
                rageManager.p1Rage += 1;
            }
            else
            {
                scoreManager.p1Score += 1;
                rageManager.p2Rage += 1;
            }
        }
        else
        {
            checkWin();
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
                        Debug.Log("**SCRIPT** one of mine got hit p1");
                    }
                }
                else // p2's turn, hits a ball
                {
                    if (isStripe) // p2's turn, hits a solid
                    {
                        cueBall.cueHitMyBall = true;
                        Debug.Log("**SCRIPT** one of mine got hit p2");
                    }
                }
            }
        }
    }

    // move this to score manager!!!!
    private void checkWin() // only gets called when 8ball is sunk
    {
        if (!scoreManager.billiardsIsP2Turn)
        {
            if (scoreManager.p1Score >= 7) // p1 is the winner
            {
                scoreManager.winner = 1;
            }
            else // p1 sunk the 8ball early
            {
                scoreManager.winner = 2;
            }

        }
        else
        {
            if (scoreManager.p2Score >= 7) // p2 is the winner
            {
                scoreManager.winner = 2;
            }
            else // p2 sunk the 8ball early
            {
                scoreManager.winner = 1;
            }
        }
    }
}
