using System.Collections;
using System.Collections.Generic;
using Coffee.UIEffects;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class GridButtons : MonoBehaviour
{
    [SerializeField]protected List<Button> buttons;
    [SerializeField]private List<TextMeshProUGUI> names;
    [SerializeField]private List<TextMeshProUGUI> descriptions;
    [SerializeField]private List<Image> images;
    public UIShiny[] shinies;

    private RectTransform RT => (transform as RectTransform);
    private GridLayoutGroup GridLayout
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

    private void Awake()
    {
        buttons.AddRange(GetComponentsInChildren<Button>());
        if (buttons.Count == 0)
        {
            AddButton();
        }
        else
        {
            ReSize();
        }
    }

    private void OnDisable()
    {
        if (shinies != null && shinies.Length > 0)
        {
            foreach (var shiny in shinies)
            {
                shiny.Stop();
                shiny.GetComponent<Image>().color = Color.white;
            }
        }
    }

    #region Public Method
    public void SetShinyColor(Color color, int idx)
    {
        shinies[idx].GetComponent<Image>().color = color;
    }


    public void SetButtonName(params string[] names)
    {
        if (names.Length < buttons.Count)
        {
            DeleteButton(buttons.Count - names.Length);
        }
        else if (names.Length > buttons.Count)
        {
            AddButton(names.Length - buttons.Count);
        }
        for (int i = 0; i < names.Length; i++)
        {
            SetName(buttons[i], names[i]);
        }
    }

    public void SetButtonAction(int idx, UnityAction action)
    {
        buttons[idx].onClick.RemoveAllListeners();
        buttons[idx].onClick.AddListener(action);
    }

    public void AddButtonAction(int idx, UnityAction action)
    {
        buttons[idx].onClick.AddListener(action);
    }

    public void SetNames(params string[] names)
    {
        for (int i = 0; i < names.Length; i++)
        {
            this.names[i].text = names[i];
        }
    }

    public void SetDecriptions(params string[] descriptions)
    {
        for (int i = 0; i < descriptions.Length; i++)
        {
            this.descriptions[i].text = descriptions[i];
        }
    }

    public void SetImages(params Sprite[] sprites)
    {
        for (int i = 0; i < sprites.Length; i++)
        {
            images[i].sprite = sprites[i];
        }
    }
    #endregion

    #region Private Method
    private void AddButton()
    {
        Button newButton = Instantiate(buttons[0], transform);
        buttons.Add(newButton);
        ReSize();
    }

    private void AddButton(int count)
    {
        for (int i = 0; i < count; i++)
        {
            AddButton();
        }
    }

    private void DeleteButton()
    {
        if (buttons.Count <= 1) return;
        int last = buttons.Count - 1;
        Destroy(buttons[last].gameObject);
        buttons.RemoveAt(last);
        ReSize();
    }

    private void DeleteButton(int count)
    {
        for (int i = 0; i < count; i++)
        {
            DeleteButton();
        }
    }

    private void ReSize()
    {
        switch(GridLayout.constraint)
        {
            case GridLayoutGroup.Constraint.Flexible:
                break;
            case GridLayoutGroup.Constraint.FixedRowCount:  //Çà °¹¼ö °íÁ¤
                if (GridLayout.constraintCount <= 1)
                    RT.sizeDelta = new Vector2(GridLayout.cellSize.x * buttons.Count + GridLayout.spacing.x * (buttons.Count - 1), GridLayout.cellSize.y);
                else
                {
                    int widthCount = Mathf.CeilToInt((float)buttons.Count / (float)GridLayout.constraintCount);
                    float width = GridLayout.cellSize.x * widthCount + GridLayout.spacing.x * (widthCount - 1);
                    float height = GridLayout.cellSize.y * GridLayout.constraintCount + GridLayout.spacing.y * (GridLayout.constraintCount - 1);
                    
                    RT.sizeDelta = new Vector2(width, height);
                }
                break;
            case GridLayoutGroup.Constraint.FixedColumnCount: //¿­ °¹¼ö °íÁ¤
                if (GridLayout.constraintCount <= 1)
                    RT.sizeDelta = new Vector2(GridLayout.cellSize.x, GridLayout.cellSize.y * buttons.Count + GridLayout.spacing.y * (buttons.Count - 1));
                else
                {
                    int heightCount = Mathf.CeilToInt((float)buttons.Count / (float)GridLayout.constraintCount);
                    float width = GridLayout.cellSize.x * GridLayout.constraintCount + GridLayout.spacing.x * (GridLayout.constraintCount - 1);
                    float height = GridLayout.cellSize.y * heightCount + GridLayout.spacing.y * (heightCount - 1);

                    RT.sizeDelta = new Vector2(width, height);
                }
                break;
        }
    }

    private void SetName(Button button, string name)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().text = name;
    }
    #endregion
}
