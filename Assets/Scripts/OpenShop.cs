using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class OpenShop : MonoBehaviour
{
    public Button openShopButton;

    public GameObject p1Shop;
    public GameObject p2Shop;

    public ScoreManager scoreManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        openShopButton.onClick.AddListener(ButtonOpenShop);

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonOpenShop()
    {
        if (!scoreManager.billiardsIsP2Turn)
        {
            p1Shop.SetActive(true);
        }
        else
        {
            p2Shop.SetActive(true);
        }
    }
}
