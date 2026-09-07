using UnityEngine;

public class NumberBall : MonoBehaviour
{
    public bool isTargetBall;
    public bool isCueBall;
    public bool is8Ball;
    public bool isStripe;
    public GameState gameState;
    public BilliardsUI billiardsUI;


    private void OnTriggerEnter2D(Collider2D collider)
    {
        // number ball goes in pocket
        if (collider.CompareTag("Pocket"))
        {
            gameObject.SetActive(false);
        }
        // checking if 8ball was sunk
        if (!is8Ball && !isCueBall)
        {
            if (isStripe)
            {
                GameState.p2BilliardsScore += 1;
                billiardsUI.UpdateBilliardsScoreUI();
                GameState.p1Rage += 1;
                billiardsUI.SetRageMeter();
				if (!GameState.isBilliardsP1Turn)
				{
					GameState.billiardsScoredThisTurn = true;
				}
			}
            else
            {
                GameState.p1BilliardsScore += 1;
                billiardsUI.UpdateBilliardsScoreUI();
				GameState.p2Rage += 1;
                billiardsUI.SetRageMeter();
				if (GameState.isBilliardsP1Turn)
                {
                    GameState.billiardsScoredThisTurn = true;
                }
			}
        }
        else if (isCueBall)
        {
            GameState.billiardsDidScratch = true;
			transform.position = new Vector3(100.0f, 0.0f, 0.0f);
			Rigidbody2D rb = GetComponent<Rigidbody2D>();
			rb.linearVelocity = Vector2.zero;
		}
        else if (is8Ball)
        {
            gameState.CheckBilliardsWinner();
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) // still need to add sfx
    {
        // number ball rage checkers
        if (GameState.state != GameState.States.BILLIARDS) return;

        NumberBall colliderBall = collision.gameObject.GetComponent<NumberBall>();

        if (collision.collider.CompareTag("NumberBall")) // if this number ball gets hit by any other ball
        {
			GameState.billiardsHitAnyBall = true;
			if (GameState.isBilliardsP1Turn)
            {
                if (!colliderBall.isStripe) // p1's turn, solid got hit
                {
                    GameState.billiardsHitOwnBall = true;
					if (isTargetBall && !GameState.billiardsGotFirstCollision)
					{
                        GameState.billiardsCorrectFirstCollision = true;
                        GameState.billiardsGotFirstCollision = true;
						Debug.Log("Correct First Collision - Solid");
					}
				}
            }
            else // p2's turn, hits a ball
            {
                if (colliderBall.isStripe) // p2's turn, hits a solid
                {
					GameState.billiardsHitOwnBall = true;
					if (isTargetBall && !GameState.billiardsGotFirstCollision)
					{
						GameState.billiardsCorrectFirstCollision = true;
						GameState.billiardsGotFirstCollision = true;
						Debug.Log("Correct First Collision - Stripe");
					}
				}
            }
        }
    }    
}
