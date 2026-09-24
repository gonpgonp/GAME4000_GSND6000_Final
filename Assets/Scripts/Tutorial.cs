using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public Animator _tutorial;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameState.billiardsTutorial = 1;
        PlayTutorial();
    }

    public void PlayTutorial()
    {
        if (GameState.billiardsTutorial == 0)
        {
            _tutorial.Play("Tutorial0"); // empty animation with no sprite
        }
        else if (GameState.billiardsTutorial == 1)
        {
            _tutorial.Play("Tutorial1");
            //GameState.billiardsTutorial++;
            return;
        }
        else if (GameState.billiardsTutorial == 2)
        {
            _tutorial.Play("Tutorial2");
            //GameState.billiardsTutorial++;
            return;
        }
        else if (GameState.billiardsTutorial == 3)
        {
            _tutorial.Play("Tutorial3");
            //GameState.billiardsTutorial++;
            return;
        }
        else if (GameState.billiardsTutorial == 4)
        {
            _tutorial.Play("Tutorial4");
            //GameState.billiardsTutorial++;
            return;
        }
        else if (GameState.billiardsTutorial == 5)
        {
            _tutorial.Play("Tutorial5");
            //GameState.billiardsTutorial = 0;
            return;
        }
    }

}
