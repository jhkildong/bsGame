using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class SelectWindow : UIComponent
{
    public GridButtons SelectButtons;
    [SerializeField]private Button UndoButton;

    private void Start()
    {
        SelectButtons = GetComponentInChildren<GridButtons>();
        if(UndoButton == null)
        {
            Transform tr = transform.Find("UndoButton");
            if(tr != null)
            {
                UndoButton = tr.GetComponent<Button>();
                UndoButton.gameObject.SetActive(false);
            }
        }
  
    }

    public void SetUndoButtonAct(UnityAction action)
    {
        UndoButton.gameObject.SetActive(true);
        UndoButton.onClick.RemoveAllListeners();
        UndoButton.onClick.AddListener(action);
    }

    public void SetUndoButtonInteract(bool state)
    {
        UndoButton.interactable = state;
    }

    public void SetUndoButtonText(string text)
    {
        UndoButton.GetComponentInChildren<TextMeshProUGUI>().text = text;
    }

    public void HideUndoButton()
    {
        UndoButton.gameObject.SetActive(false);
    }

    public void ExitButton()
    {
        gameObject.SetActive(false);
    }
}
