using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(GridLayoutGroup))]
public class BlessIconsUI : UIComponent
{
    [SerializeField] private Image defaultIcon;
    Dictionary<int, Image> iconDict = new Dictionary<int, Image>();    // Key: BlessID, Value: Icon
    private RectTransform RT => (transform as RectTransform);

    private void Awake()
    {
        gameObject.SetActive(false);
    }


    public void SetJobBlessIcon(BlessData data)
    {
        if (data.ID > (int)BlessID.MAGE || data.ID < (int)BlessID.WARRIOR)
            return;
        defaultIcon.sprite = data.Icon;
        defaultIcon.GetComponentInChildren<TextMeshProUGUI>().text = "";
        iconDict.Add(data.ID, defaultIcon);
        gameObject.SetActive(true);
    }

    public void AddBlessIcon(BlessData data)
    {
        Image newIcon = Instantiate(defaultIcon, transform);
        if (data.Icon == null)
            newIcon.sprite = null;
        else
            newIcon.sprite = data.Icon;
        iconDict.Add(data.ID, newIcon);
        newIcon.GetComponentInChildren<TextMeshProUGUI>().text = "";
    }

    public void DeleteIcon(int dataID)
    {
        if (iconDict.Count <= 1) return;
        Destroy(iconDict[dataID].gameObject);
        iconDict.Remove(dataID);
    }

    public void SetText(string text, int ID)
    {
        iconDict[ID].GetComponentInChildren<TextMeshProUGUI>().text = text;
    }
    
}
