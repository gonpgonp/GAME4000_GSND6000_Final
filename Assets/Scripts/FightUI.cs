using UnityEngine;
using UnityEngine.UI;

public class FightUI : MonoBehaviour
{
    public Animator p1Score;
    public Animator p2Score;

	public Image dial;

	void Awake()
	{
		dial.fillAmount = .5f;
	}

    public void SetScoreUI()
    {
        /*int score = GameState.p1FightScore;
		if (score < 0 || score > 5)
        {
			score = 0;
        }
		string anim = "DickSwag" + score.ToString();
		p1Score.Play(anim);

		score = GameState.p2FightScore;
		if (score < 0 || score > 5)
		{
			score = 0;
		}
		anim = "RichardSwag" + score.ToString();
		p2Score.Play(anim);*/

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


	}
}
