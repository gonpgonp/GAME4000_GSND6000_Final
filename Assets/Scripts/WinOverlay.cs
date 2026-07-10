using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinOverlay : MonoBehaviour
{
    public ScoreManager scoreManager;
    public GameObject dickWin;
    public GameObject richardWin;
    public GameObject restartButton;
    public GameObject darken;

    void Start()
    {
        DisplayCorrectWinOverlay();
        SetRestartButton();
    }

    public void DisplayCorrectWinOverlay()
    {
        darken.SetActive(true);
        SetRestartButton();

        if (scoreManager.winner == 1)
        {
            dickWin.SetActive(true);
        }
        else if (scoreManager.winner == 2)
        {
            richardWin.SetActive(true);
        }
    }

    public void SetRestartButton()
    {
        restartButton.SetActive(true);
        Button btn = restartButton.GetComponent<Button>();
        btn.onClick.AddListener(RestartGame);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Week1Proto");
    }
}
