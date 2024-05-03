using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridImages : UIComponent
{
    [SerializeField] protected List<Image> icons;
    private List<int> iconsIdList = new List<int>();
    private RectTransform RT => (transform as RectTransform);
    private GridLayoutGroup GridLayout
    {
        get
        {
            if (_gridLayout == null)
            {
                _gridLayout = GetComponent<GridLayoutGroup>();
            }
            return _gridLayout;
        }
    }
    private GridLayoutGroup _gridLayout;

    private void Awake()
    {
        icons.AddRange(GetComponentsInChildren<Image>());
        if (icons.Count == 0)
        {
            AddIcon();
        }
        else
        {
            ReSize();
        }
    }

    public void AddIcon()
    {
        Image newIcon = Instantiate(icons[0], transform);
        icons.Add(newIcon);
        ReSize();
    }

    public void AddIcon(Sprite icon)
    {
        Image newIcon = Instantiate(icons[0], transform);
        if(icon != null)
            newIcon.sprite = icon;
        icons.Add(newIcon);
        ReSize();
    }

    public void AddIcon(params Sprite[] icons)
    {
        foreach (Sprite icon in icons)
        {
            AddIcon(icon);
        }
    }

    public void SetJobBlessIcon(BlessData data)
    {
        if(data.ID > (int)BlessID.MAGE || data.ID < (int)BlessID.WARRIOR)
            return;
        icons[0].sprite = data.Icon;
        iconsIdList.Add(data.ID);
    }

    public void AddBlessIcon(BlessData data)
    {
        Image newIcon = Instantiate(icons[0], transform);
        newIcon.sprite = data.Icon;
        iconsIdList.Add(data.ID);
        icons.Add(newIcon);
        ReSize();
    }

    public void DeleteIcon()
    {
        if (icons.Count <= 1) return;
        int last = icons.Count - 1;
        Destroy(icons[last].gameObject);
        icons.RemoveAt(last);
        ReSize();
    }

    public void DeleteIconAt(int ID)
    {
        if (icons.Count <= 1) return;
        int idx = iconsIdList.IndexOf(ID);
        Destroy(icons[idx].gameObject);
        icons.RemoveAt(idx);
        ReSize();
    }

    public void DeleteIcon(int count)
    {
        for (int i = 0; i < count; i++)
        {
            DeleteIcon();
        }
    }

    public void SetTextAt(string text, int ID)
    {
        int idx = iconsIdList.IndexOf(ID);
        icons[idx].GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    private void ReSize()
    {
        switch (GridLayout.constraint)
        {
            case GridLayoutGroup.Constraint.Flexible:
                break;
            case GridLayoutGroup.Constraint.FixedRowCount:  //Çà °¹¼ö °íÁ¤
                if (GridLayout.constraintCount <= 1)
                    RT.sizeDelta = new Vector2(GridLayout.cellSize.x * icons.Count + GridLayout.spacing.x * (icons.Count - 1), GridLayout.cellSize.y);
                else
                {
                    int widthCount = Mathf.CeilToInt((float)icons.Count / (float)GridLayout.constraintCount);
                    float width = GridLayout.cellSize.x * widthCount + GridLayout.spacing.x * (widthCount - 1);
                    float height = GridLayout.cellSize.y * GridLayout.constraintCount + GridLayout.spacing.y * (GridLayout.constraintCount - 1);

                    RT.sizeDelta = new Vector2(width, height);
                }
                break;
            case GridLayoutGroup.Constraint.FixedColumnCount: //¿­ °¹¼ö °íÁ¤
                if (GridLayout.constraintCount <= 1)
                    RT.sizeDelta = new Vector2(GridLayout.cellSize.x, GridLayout.cellSize.y * icons.Count + GridLayout.spacing.y * (icons.Count - 1));
                else
                {
                    int heightCount = Mathf.CeilToInt((float)icons.Count / (float)GridLayout.constraintCount);
                    float width = GridLayout.cellSize.x * GridLayout.constraintCount + GridLayout.spacing.x * (GridLayout.constraintCount - 1);
                    float height = GridLayout.cellSize.y * heightCount + GridLayout.spacing.y * (heightCount - 1);

                    RT.sizeDelta = new Vector2(width, height);
                }
                break;
        }
    }

    
}
