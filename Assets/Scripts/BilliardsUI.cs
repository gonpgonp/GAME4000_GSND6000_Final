using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BilliardsUI : MonoBehaviour
{
    public ScoreManager scoreManager;

    public RageManager rageManager;
    public Image p1RageBar;
    public Image p2RageBar;
    public GameObject startFightButton;

    private Color32 rageColor = new Color32(255, 44, 0, 255);
    private Color32 calmColor = new Color32(255, 255, 255, 255);

    public TextMeshProUGUI p1ScoreTMP;
    public TextMeshProUGUI p2ScoreTMP;

    void Start()
    {
        SetRageMeter();
    }

    public void SetRageMeter()
    {
        float p1Rage = rageManager.p1Rage;
        float p2Rage = rageManager.p2Rage;

        p1RageBar.rectTransform.localScale = new Vector3(.4f, p1Rage/25, .4f);
        p2RageBar.rectTransform.localScale = new Vector3(.4f, p2Rage/25, .4f);
        // want to fine-tune this behavior to look a little nicer later

        if (!scoreManager.billiardsIsP2Turn) // set p1 rage UI
        {
            if (rageManager.p1Rage >= rageManager.minimumFightRage)
            {
                p1RageBar.color = rageColor;
                // play pissed message anim and dickheadpissed anim
                startFightButton.SetActive(true);
            }
            else
            {
                p1RageBar.color = calmColor;
                //play pissed messages no one anim and dickheadcalm anim
                startFightButton.SetActive(false);
            }
        }
        else // set p2 rage UI
        {
            if (rageManager.p2Rage >= rageManager.minimumFightRage)
            {
                p2RageBar.color = rageColor;
                // play pissed message anim and dickheadpissed anim
                startFightButton.SetActive(true);
            }
            else
            {
                p2RageBar.color = calmColor;
                //play pissed messages no one anim and dickheadcalm anim
                startFightButton.SetActive(false);
            }
        }
    }

    public void UpdateBilliardsScoreUI()
    {
        p1ScoreTMP.text = scoreManager.p1Score.ToString();
        p2ScoreTMP.text = scoreManager.p2Score.ToString();
    }
}
