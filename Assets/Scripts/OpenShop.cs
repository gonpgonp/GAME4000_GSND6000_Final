using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class OpenShop : MonoBehaviour
{
    public Button openShopButton;

    public GameObject p1Shop;
    public GameObject p2Shop;

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
        if (GameState.isBilliardsP1Turn && !GameState.isShopOpen)
        {
            p1Shop.SetActive(true);
            GameState.isShopOpen = true;
        }
        else if (GameState.isBilliardsP1Turn && GameState.isShopOpen)
        {
            p1Shop.SetActive(false);
            GameState.isShopOpen = false;
        }
        else if (!GameState.isBilliardsP1Turn && !GameState.isShopOpen)
        {
            p2Shop.SetActive(true);
            GameState.isShopOpen = true;
        }
        else if (!GameState.isBilliardsP1Turn && GameState.isShopOpen)
        {
            p2Shop.SetActive(false);
            GameState.isShopOpen = false;
        }
    }
}
