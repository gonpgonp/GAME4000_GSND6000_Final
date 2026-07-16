using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class FightUI : MonoBehaviour
{
    public Animator p1Score;
    public Animator p2Score;
    public GameState gameState;

    public void SetScoreUI()
    {
        int score = gameState.p1FightScore;
		if (score < 0 || score > 5)
        {
			score = 0;
        }
		string anim = "DickSwag" + score.ToString();
		p1Score.Play(anim);

		score = gameState.p2FightScore;
		if (score < 0 || score > 5)
		{
			score = 0;
		}
		anim = "RichardSwag" + score.ToString();
		p2Score.Play(anim);
	}
}
