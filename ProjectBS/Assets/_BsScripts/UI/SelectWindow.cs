using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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

    public void SetUndoButton(UnityAction action)
    {
        UndoButton.gameObject.SetActive(true);
        UndoButton.onClick.RemoveAllListeners();
        UndoButton.onClick.AddListener(action);
    }

    public void HideUndoButton()
    {
        UndoButton.gameObject.SetActive(false);
    }
}
