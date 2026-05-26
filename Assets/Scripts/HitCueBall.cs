using UnityEngine;
using UnityEngine.InputSystem;

public class HitCueBall : MonoBehaviour
{
    public InputActionReference click;
    public bool clickedOnBall = false;
    void OnEnable()
    {
        click.action.Enable();
        click.action.canceled += OnClickReleased;
    }

    void OnDisable()
    {
        click.action.canceled -= OnClickReleased;
        click.action.Disable();
    }

    private void OnClickReleased(InputAction.CallbackContext ctx)
    {
        Debug.Log("mouse released");
    }

    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector2 mousePosCam = Camera.main.ScreenToWorldPoint(mousePos);
        Vector2 cueBallPos = transform.position;

        // check if clicked on cue ball
        if (click.action.triggered)
        {
            //need to add if(!isFight), if(!shopOpen), and if(!hasHit) || if(secondTapAvailable)
            float dist = Vector2.Distance(mousePosCam, cueBallPos);
            if (dist < .5f)
            {
                clickedOnBall = true;
            }
            else
            {
                clickedOnBall = false;
            }

        }

    }
}
