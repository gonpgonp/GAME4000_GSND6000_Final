using Unity.VisualScripting;
using UnityEngine;

public class CameraHandler : MonoBehaviour
{
    private Camera self;
    private Vector3 targetLocation = new Vector3(0, 0, -10);
	private float targetSize = 6.5f;

	// Start is called once before the first execution of Update after the MonoBehaviour is created
	void Start()
    {
		self = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        SmoothSize();
		SmoothPosition();
	}

    void SmoothSize()
    {
		if (self.orthographicSize != targetSize)
		{
			float diff = (targetSize - self.orthographicSize) / 10.0f;
			self.orthographicSize += diff;
			if (Mathf.Abs(diff) < 0.1)
			{
				self.orthographicSize = targetSize;
			}
		}
	}

	void SmoothPosition()
	{
		if (this.transform.position != targetLocation)
		{
			Vector3 diff = (targetLocation - this.transform.position) / 10.0f;
			this.transform.position += diff;
			if (Mathf.Abs(diff.magnitude) < 0.1)
			{
				this.transform.position = targetLocation;
			}
		}
	}

    public void SetTarget(Vector3 _location, float _size)
    {
        targetLocation = _location;
        targetSize = _size;
    }
}
