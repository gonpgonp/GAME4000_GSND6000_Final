using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FightUI : MonoBehaviour
{
    public Animator p1Score;
    public Animator p2Score;

	public TextMeshProUGUI timer;

	public Image dial;

	public Image dialArrow;


	private void FixedUpdate()
	{
		timer.text = Mathf.Ceil(GameState.fightTimer).ToString();
	}

	public void SetScoreUI()
    {
		float scoreRatio = (GameState.fightScore + 5) / 10.0f;
		Debug.Log("Dial - Score ratio: " + scoreRatio);
		dial.fillAmount = scoreRatio;

		float dialAngle = 90 - (dial.fillAmount * 180);
		
		if (dialAngle == 90) // visually adjusting full angles so they don't get cut off at the bottom
		{
			dialAngle = 80;
		}
		else if (dialAngle == -90)
		{
			dialAngle = -80;
		}

		RectTransform rt = dialArrow.rectTransform;
		rt.localRotation = Quaternion.Euler(0, 0, dialAngle);
	}
}
