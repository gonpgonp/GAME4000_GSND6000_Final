using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    // hotbarSkills is a list that describes the skills in the order they were bought
    // (skills just get appended to the end each time they're bought)

    public List<SkillButton> hotbarSkills = new List<SkillButton>();
    
    // hotbarSkillOrganized takes hotbarSkills and organizes them to display correctly
    List<SkillButton> hotbarSkillsOrganized = new List<SkillButton>();

    // hotbar[] is the array of child slot objects (Slot1, Slot2, Slot3...)
    public Transform[] hotbar;

    public ScoreManager scoreManager;

    void Start()
    {
        
    }

    public void OrganizeHotbarSkills()
    {
        // clear the organized list every time in order to slot in the new one
        // this ASSUMES that skills are bought in 123 order (which they always are)

        hotbarSkillsOrganized.Clear();

        for (int i=0; i<hotbarSkills.Count; i++)
        {
            string name = hotbarSkills[i].name;
            if (name.Contains("Cue"))
            {
                hotbarSkillsOrganized.Add(hotbarSkills[i]);
            }
        }

        for (int i=0; i<hotbarSkills.Count; i++)
        {
            string name = hotbarSkills[i].name;
            if (name.Contains("Ball"))
            {
                hotbarSkillsOrganized.Add(hotbarSkills[i]);
            }
        }
        
        for (int i=0; i<hotbarSkills.Count; i++)
        {
            string name = hotbarSkills[i].name;
            if (name.Contains("Table"))
            {
                hotbarSkillsOrganized.Add(hotbarSkills[i]);
            }
        }
    }

    public void DisplayHotbarSkills()
    {
        ClearHotbar();

        for (int i=0; i<hotbarSkillsOrganized.Count; i++)
        {
            SkillButton sb = Instantiate(hotbarSkillsOrganized[i], hotbar[i], false);

            sb.state = 4;
            sb.SetButtonBehavior();
            
            sb.GetComponent<RectTransform>().anchoredPosition = new Vector2(0f, 0f);
            sb.GetComponent<RectTransform>().sizeDelta = new Vector2(50f, 50f);
            
        }
    }

    public void ClearHotbar()
    {
        for (int i=0; i<transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            for (int j=child.childCount-1; j>=0; j--)
            {
                GameObject grandchild = child.GetChild(j).gameObject;
                Destroy(grandchild);
            }
        }        
    }
}
