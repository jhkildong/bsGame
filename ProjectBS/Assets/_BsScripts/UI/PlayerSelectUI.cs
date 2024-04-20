using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerSelectUI : GridButtons
{
    void Awake()
    {
        buttons.AddRange(GetComponentsInChildren<Button>());
    }

    public void SetSelectButtons(string[] names)
    {
        if(names.Length < buttons.Count)
        {
            DeleteButton(buttons.Count - names.Length);
        }
        else if(names.Length > buttons.Count)
        {
            AddButton(names.Length - buttons.Count);
        }
        for(int i = 0; i < names.Length; i++)
        {
            SetName(buttons[i], names[i]);
        }
    }

    public void SetButtonAction(int idx, UnityAction action)
    {
        buttons[idx].onClick.RemoveAllListeners();
        buttons[idx].onClick.AddListener(action);
    }
}
