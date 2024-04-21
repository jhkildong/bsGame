using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GridButtons : MonoBehaviour
{
    [SerializeField]protected List<Button> buttons;
    protected RectTransform RT => (transform as RectTransform);
    protected GridLayoutGroup GridLayout
    {
        get
        {
            if(_gridLayout == null)
            {
                _gridLayout = GetComponent<GridLayoutGroup>();
            }
            return _gridLayout;
        }
    }
    private GridLayoutGroup _gridLayout;

    protected void AddButton()
    {
        Button newButton = Instantiate(buttons[0], transform);
        buttons.Add(newButton);
        Reposition(true);
    }

    protected void AddButton(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddButton();
        }
    }

    protected void DeleteButton()
    {
        if (buttons.Count <= 1) return;
        int last = buttons.Count - 1;
        Destroy(buttons[last].gameObject);
        buttons.RemoveAt(last);
        Reposition(false);
    }

    protected void DeleteButton(int count)
    {
        for (int i = 0; i < count; i++)
        {
            DeleteButton();
        }
    }

    /// <summary> true : Add, false : Delete </summary>
    protected virtual void Reposition(bool flag)
    {
        //기본 중앙 정렬
        if (GridLayout.startAxis == GridLayoutGroup.Axis.Vertical)
        {
            float width = (GridLayout.cellSize.x + GridLayout.spacing.x) * 0.5f;
            width = flag ? -width : width; // true : Add, false : Delete
            RT.anchoredPosition += new Vector2(width, 0);
        }
        else // Horizontal
        {
            float height = (GridLayout.cellSize.y + GridLayout.spacing.y) * 0.5f;
            height = flag ? -height : height; // true : Add, false : Delete
            RT.anchoredPosition += new Vector2(0, height);
        }
    }
    

    protected void SetName(Button button, string name)
    {
        button.name = name;
        button.GetComponentInChildren<TextMeshProUGUI>().text = name;
    }
}
