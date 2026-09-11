using UnityEngine;
using UnityEngine.UI;

public class OpenRules : MonoBehaviour
{
    public GameObject rules;

    public GameObject dimBg;
    public Button openRules;
    bool rulesOpen = false;
    void Start()
    {
        openRules.onClick.AddListener(OpenCloseRules);
    }

    void OpenCloseRules()
    {
        if (!rulesOpen)
        {
            rules.SetActive(true);
            dimBg.SetActive(true);
            rulesOpen = true;
        }
        else
        {
            rules.SetActive(false);
            dimBg.SetActive(false);
            rulesOpen = false;
        }
    }
}
