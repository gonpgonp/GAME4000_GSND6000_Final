using System.Security.Cryptography;
using Unity.VisualScripting;
using Unity.VisualScripting.InputSystem;
using UnityEngine;
using UnityEngine.InputSystem;

public class CueBall : MonoBehaviour
{
	public GameState gameState;
	public GameObject cue;
	public GameObject powerUpHandler;
	//public bool hasHit;
	//public bool secondTapAvailable;
	//public bool cueHitAnyBall = false;
	//public bool cueHitMyBall = false;
	//public bool hasBroken = false;

    //bool didScratch;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("NumberBall"))
        {
            //hasBroken = true;
            GameState.billiardsHitAnyBall = true;
        }
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.tag == "Pocket")
        {
            transform.position = new Vector3( 100.0f, 0.0f, 0.0f );
			Rigidbody2D rb = GetComponent<Rigidbody2D>();
			if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
				GameState.billiardsDidScratch = true;
            }
		}
	}
}
