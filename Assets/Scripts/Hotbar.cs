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

    /* hotbarSkillsUsable takes each SkillButton in hotbarSkillOrganized and adds the
    corresponding hotbarSkillButton */
    List<Button> hotbarSkillsUsable;

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
}
