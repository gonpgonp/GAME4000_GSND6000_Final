using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Crowd : MonoBehaviour
{
    public Sprite[] crowdSprites;

    List<Sprite> leftCrowdList = new List<Sprite>();
    public GameObject[] leftCrowd;

    List<Sprite> rightCrowdList = new List<Sprite>();
    public GameObject[] rightCrowd;

    void Start()
    {
        ClearCrowds();
        // set up first five crowd sprites in the left list, second five in the right list
        // might change to be dynamic
        for (int i=0; i<5; i++)
        {
            leftCrowdList.Add(crowdSprites[i]);
        }

        for (int j=5; j<10; j++)
        {
            rightCrowdList.Add(crowdSprites[j]);
        }

        DisplayCrowd();
    }

    public void UpdateCrowd()
    {
        DistributeCrowdLists();
        DisplayCrowd();
    }

    public void DistributeCrowdLists()
    {
        ClearCrowdLists(); // clear the crowd lists so they can be redistributed correctly based on score

        int fightRatio = 5;
        float p1Score = 1;
        float p2Score = 1;

        if (GameState.p1FightScore != 0)
        {
            p1Score = GameState.p1FightScore;
        }

        if (GameState.p2FightScore != 0)
        {
            p2Score = GameState.p2FightScore;
        }

        float f = p1Score / (p1Score + p2Score);
        f *= 10;
        fightRatio = Mathf.RoundToInt(f);

        Debug.Log("p1score: " + p1Score);
        Debug.Log("p2score: " + p2Score);
        Debug.Log("fightRatio: " + fightRatio);
        
        for (int i=0; i<fightRatio; i++)
        {
            leftCrowdList.Add(crowdSprites[i]);
        }

        for (int j=fightRatio; j<10; j++)
        {
            rightCrowdList.Add(crowdSprites[j]);
        }
    }

    public void ClearCrowdLists() // resets the crowd lists
    {
        leftCrowdList.Clear();
        rightCrowdList.Clear();
    }

    public void ClearCrowds() // resets the crowd sprites so they can update correctly
    {
        for (int i=0; i<10; i++)
        {
            Image imgL = leftCrowd[i].GetComponent<Image>();
            Image imgR = rightCrowd[i].GetComponent<Image>();

            imgL.sprite = null;
            imgL.color = new Color32(0, 0, 0, 0);
            imgR.sprite = null;
            imgR.color = new Color32(0, 0, 0, 0);
        }
    }

    public void DisplayCrowd() // takes leftcrowdlist and rightcrowdlist and puts em in the gameobject slots
    {
       ClearCrowds();

        for (int i=0; i<leftCrowdList.Count; i++)
        {
            Image img = leftCrowd[i].GetComponent<Image>();
            img.sprite = leftCrowdList[i];
            img.color = new Color32(255, 255, 255, 255);
            img.SetNativeSize();
            
        }

        for (int j=0; j<rightCrowdList.Count; j++)
        {
            Image img = rightCrowd[j].GetComponent<Image>();
            img.sprite = rightCrowdList[j];
            img.color = new Color32(255, 255, 255, 255);
            img.SetNativeSize();
        }
    }
}
