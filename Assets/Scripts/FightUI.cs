using UnityEngine;
using UnityEngine.UI;

public class FightUI : MonoBehaviour
{
    public Animator p1Score;
    public Animator p2Score;

	public Image dial;

	public Image dialArrow;


    public void SetScoreUI()
    {
    	float p1Score = GameState.p1FightScore;
		float totalScore = GameState.p1FightScore + GameState.p2FightScore;

		if (totalScore != 0)
		{
			float scoreRatio = p1Score / totalScore;
			dial.fillAmount = scoreRatio;
		}
		else
		{
			dial.fillAmount = .5f;
		}

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
