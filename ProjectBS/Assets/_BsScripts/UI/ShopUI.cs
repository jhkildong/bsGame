using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopUI : UIComponent
{
    public void OnBackButton()
    {
        gameObject.SetActive(false);
    }
}
