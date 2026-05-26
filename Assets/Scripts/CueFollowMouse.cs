using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.InputSystem;
public class CueFollowMouse : MonoBehaviour
{
   public GameObject cueBall;
   public float maxCueDistance = 2.5f;
   float xDiff;
   float yDiff;

   public InputActionReference click;
   SpriteRenderer spriteRenderer;

    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        // set cue position to mouse
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 mousePosCam = Camera.main.ScreenToWorldPoint(mousePos);
        mousePosCam.z = 0;
        transform.position = mousePosCam;

        // set distance to fixed max amount away from cue ball pos
        Vector3 cuePos = cueBall.transform.position;
        Vector3 v = mousePosCam - cuePos;

        if (v.magnitude > maxCueDistance)
        {
            transform.position = (v.normalized * maxCueDistance) + cuePos;
        }

        // rotate cue around cue ball
        xDiff = cuePos.x - mousePosCam.x;
        yDiff = cuePos.y - mousePosCam.y;

        if (xDiff < 0)
        {
            float f = (Mathf.Atan(yDiff/xDiff) * (180/Mathf.PI)) + 180;
            Quaternion q = Quaternion.Euler(0, 0, f);
            transform.rotation = q;
        }
        else
        {
            float f = Mathf.Atan(yDiff/xDiff) * (180/Mathf.PI);
            Quaternion q = Quaternion.Euler(0, 0, f);
            transform.rotation = q;
        }

        // set transparency
        // need to add if statement for ShopOpen or DoingClickPowerUp (vars don't exist yet)
       if (click.action.IsPressed())
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);
        }
        else
        {
            spriteRenderer.color = new Color(1f, 1f, 1f, .3f);

        }
        
    }
}
