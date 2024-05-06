using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowUI : UIComponent
{
    public void OnBackButton()
    {
        gameObject.SetActive(false);
    }
}
