using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPlay()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/5002_PlayerSelectWindow"), transform); 
    }

    public void OnShop()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/ShopUI"), transform); 
    }

    public void OnSettings()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/SettingsUI"), transform); 
    }
    public void OnCredits()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("Prefabs/UI/CreditsUI"), transform);  
    }
    public void OnQuit()
    {
        Application.Quit();
    }

}
