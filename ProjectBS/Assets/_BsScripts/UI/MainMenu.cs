using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private UIComponent PlayerSelectWindow;
    [SerializeField] private UIComponent ShopUI;
    [SerializeField] private UIComponent SettingsUI;
    [SerializeField] private UIComponent CreditsUI;
    

    [SerializeField] Button playButton;
    [SerializeField] Button shopButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button creditsButton;
    [SerializeField] Button QuitButton;

    private void Start()
    {
        PlayerSelectWindow.gameObject.SetActive(false);
        ShopUI.gameObject.SetActive(false);
        SettingsUI.gameObject.SetActive(false);
        CreditsUI.gameObject.SetActive(false);

        playButton.onClick.AddListener(OnPlay);
        shopButton.onClick.AddListener(OnShop);
        settingsButton.onClick.AddListener(OnSettings);
        creditsButton.onClick.AddListener(OnCredits);
        QuitButton.onClick.AddListener(OnQuit);

    }

    private void OnPlay()
    {
        PlayerSelectWindow.gameObject.SetActive(true);
        PlayerSelectWindow.GetComponent<PlayerSelectUI>().SetJobSelect();
    }

    private void OnShop()
    {
        ShopUI.gameObject.SetActive(true);
    }

    private void OnSettings()
    {
        SettingsUI.gameObject.SetActive(true);
    }
    private void OnCredits()
    {
        CreditsUI.gameObject.SetActive(true);
    }
    private void OnQuit()
    {
        Application.Quit();
    }

}
