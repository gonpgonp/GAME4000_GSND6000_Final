using UnityEngine;

public class NumberBall : MonoBehaviour
{    
    private bool is8Ball;
    private bool isStripe;

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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // number ball rage checkers

        if (cueBall.hasBroken)
        {
            Debug.Log("ya BROKE");
        }
    }

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
