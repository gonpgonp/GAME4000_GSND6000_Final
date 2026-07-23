using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FightUI : MonoBehaviour
{
    public Animator p1Score;
    public Animator p2Score;

    public void SetScoreUI()
    {
        int score = GameState.p1FightScore;
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
		p2Score.Play(anim);
	}
}
