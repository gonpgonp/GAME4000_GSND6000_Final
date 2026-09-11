using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class OpenShop : MonoBehaviour
{
    public Button openShopButton;

    public GameObject p1Shop;
    public GameObject p2Shop;

    public GameObject dimBg;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        openShopButton.onClick.AddListener(ButtonOpenCloseShop);

    }
    // Update is called once per frame
    void Update()
    {
        
    }

    void ButtonOpenCloseShop()
    {
        if (GameState.billiardsP1Turn && !GameState.isShopOpen)
        {
            p1Shop.SetActive(true);
            dimBg.SetActive(true);
            GameState.isShopOpen = true;
        }
        else if (GameState.billiardsP1Turn && GameState.isShopOpen)
        {
            p1Shop.SetActive(false);
            dimBg.SetActive(false);
            GameState.isShopOpen = false;
        }
        else if (!GameState.billiardsP1Turn && !GameState.isShopOpen)
        {
            p2Shop.SetActive(true);
            dimBg.SetActive(true);
            GameState.isShopOpen = true;
        }
        else if (!GameState.billiardsP1Turn && GameState.isShopOpen)
        {
            p2Shop.SetActive(false);
            dimBg.SetActive(false);
            GameState.isShopOpen = false;
        }
    }
}
