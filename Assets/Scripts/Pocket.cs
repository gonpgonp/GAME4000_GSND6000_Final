using UnityEngine;

public class Pocket : MonoBehaviour
{

    public GameObject gravity;
    public GameObject blocker;

    float gravityStrength = 3.0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	private void OnTriggerStay2D(Collider2D collision)
	{
        Vector2 pull = (transform.position - collision.transform.position).normalized * gravityStrength;
        collision.GetComponent<Rigidbody2D>().AddForce(pull);
	}

	public void SetGravity(bool b)
    {
        gravity.SetActive(b);
    }

	public void SetBlocker(bool b)
	{
		blocker.SetActive(b);
	}
}
