using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopUpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform tooltipText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipText.gameObject.SetActive(true); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipText.gameObject.SetActive(false);
    }
}
