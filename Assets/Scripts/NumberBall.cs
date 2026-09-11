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
		if (isCueBall)
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
        else
        {
            if (!GameState.billiardsBallsSelected)
            {
                GameState.billiardsP1Solids = GameState.billiardsP1Turn != isStripe;
                GameState.billiardsBallsSelected = true;
                Debug.Log("P1 is solids : " + GameState.billiardsP1Solids);
            }
            gameState.SinkBall(isStripe);
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
			if (GameState.billiardsP1Turn)
            {
                if (GameState.billiardsBallsSelected && GameState.billiardsP1Solids != colliderBall.isStripe)
                {
                    GameState.billiardsHitOwnBall = true;
					if (isTargetBall && !GameState.billiardsGotFirstCollision)
					{
                        GameState.billiardsCorrectFirstCollision = true;
                        GameState.billiardsGotFirstCollision = true;
						Debug.Log("Correct First Collision - P1");
					}
				}
            }
            else // p2's turn, hits a ball
            {
                if (GameState.billiardsBallsSelected && !GameState.billiardsP1Solids != colliderBall.isStripe)
                {
					GameState.billiardsHitOwnBall = true;
					if (isTargetBall && !GameState.billiardsGotFirstCollision)
					{
						GameState.billiardsCorrectFirstCollision = true;
						GameState.billiardsGotFirstCollision = true;
						Debug.Log("Correct First Collision - P2");
					}
				}
            }
        }
    }    
}
