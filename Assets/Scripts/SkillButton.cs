using UnityEngine;
using UnityEngine.UI;

public class SkillButton : MonoBehaviour
{
    // 0 = avail, 1 = unavail, 2 = avail but can't afford, 3 = bought
    public int state = 0;
    
    public Sprite availSprite;
    public Sprite cantAffordSprite;
    public Sprite unavailSprite;
    public Sprite boughtSprite;

    void Start()
    {
        SetButtons();
    }

    void SetButtons()
    {
        Button button = this.GetComponent<Button>();

        if (state == 0)
        {
            button.image.sprite = availSprite;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(BuySkill);
        }
        else if (state == 1)
        {
            button.image.sprite = unavailSprite;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Unavail);
        }
        else if (state == 2)
        {
            button.image.sprite = cantAffordSprite;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(CantAfford);
        }
        else if (state == 3)
        {
            button.image.sprite = boughtSprite;
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(Bought);
        }
    }

    void BuySkill()
    {
        Debug.Log("you bought the skill");
        state = 3;
        SetButtons();
    }

    void Unavail()
    {
        Debug.Log("this isn't available");
    }

    void CantAfford()
    {
        Debug.Log("this is avail but you can't afford it");
    }

    void Bought()
    {
        Debug.Log("you already bought this");
    }
}
