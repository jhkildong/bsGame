using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopUpgradeButton : MonoBehaviour
{
    public Transform changeColorList;
    public Transform changeImageList;

    private List<Image> unUpgradeImage = new List<Image>();
    private List<Image> changeImage = new List<Image>();

    private Button button;
    private int currentIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        foreach (Transform child in changeColorList)
        {
            Image iamge = child.GetComponent<Image>();
            if(iamge != null )
            {
                unUpgradeImage.Add(iamge);
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
}
