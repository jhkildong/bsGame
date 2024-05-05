using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject PlayerSelectWindow;
    private GameObject ShopUI;
    private GameObject SettingsUI;
    private GameObject CreditsUI;

    public void OnPlay()
    {
        if (PlayerSelectWindow == null)
        {
            PlayerSelectWindow = Instantiate(Resources.Load<GameObject>("Prefabs/UI/5002_PlayerSelectWindow"), transform);
        }
        PlayerSelectWindow.SetActive(true);
    }

    public void OnShop()
    {
        if (ShopUI == null)
        {
            ShopUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopUI"), transform);
        }
        ShopUI.SetActive(true);
    }

    public void OnSettings()
    {
        if (SettingsUI == null)
        {
            SettingsUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/SettingsUI"), transform);
        }
        SettingsUI.SetActive(true);
    }
    public void OnCredits()
    {
        if (CreditsUI == null)
        {
            CreditsUI = Instantiate(Resources.Load<GameObject>("Prefabs/UI/CreditsUI"), transform);
        }
        CreditsUI.SetActive(true);
    }
    public void OnQuit()
    {
        Application.Quit();
    }

}
