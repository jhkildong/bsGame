using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopUpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Transform OriginImageList;
    public Transform changeImageList;
    public Transform tooltipText;

    private List<Image> OriginImage = new List<Image>();
    private List<Image> changeImage = new List<Image>();

    private Button button;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in OriginImageList)
        {
            Image iamge = child.GetComponent<Image>();
            if(iamge != null )
            {
                OriginImage.Add(iamge);
            }
        }

        foreach (Transform child in changeImageList)
        {
            Image iamge = child.GetComponent<Image>();
            if (iamge != null)
            {
                changeImage.Add(iamge);
            }
        }

        button = GetComponentInChildren<Button>();
        button.onClick.AddListener(ChangeImage);
    }

    void ChangeImage()
    {
        if(currentIndex < changeImage.Count)
        {
            changeImage[currentIndex].gameObject.SetActive(true);
            currentIndex++;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tooltipText.gameObject.SetActive(true); 
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipText.gameObject.SetActive(false);
    }
}
