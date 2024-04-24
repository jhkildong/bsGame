using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerSelectWindow : UIComponent
{
    public override int ID => _id;
    [SerializeField] private int _id = 5002;
    public PlayerSelectUI playerSelectUI;
    [SerializeField]private Button UndoButton;

    private void Start()
    {
        playerSelectUI = GetComponentInChildren<PlayerSelectUI>();
        if(UndoButton == null)
        {
            UndoButton = transform.Find("UndoButton").GetComponent<Button>();
        }
        UndoButton.gameObject.SetActive(false);
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
