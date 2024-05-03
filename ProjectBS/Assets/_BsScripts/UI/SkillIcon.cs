using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillIcon : UIComponent
{
    [SerializeField]private Image _icon;
    [SerializeField] private TextMeshProUGUI _coolTimeText;
    [SerializeField] private TextMeshProUGUI _stackText;

    private void Start()
    {
        GameManager.Instance.Player.ChangeCoolTimeAct += ChangeImg;
        GameManager.Instance.Player.ChangeCoolTimeAct += ChangeText;
        GameManager.Instance.Player.ChangeStackAct += ChangeStack;

        ChangeImg(0, 1);
        ChangeText(0, 1);
        ChangeStack(-1);
    }

    private void ChangeImg(float CurCoolTime, float MaxCoolTime)
    {
        if(CurCoolTime <= 0)
        {
            _icon.fillAmount = 0;
            return;
        }
        _icon.fillAmount = CurCoolTime / MaxCoolTime;
    }

    private void ChangeText(float CurCoolTime, float MaxCoolTime)
    {
        if(CurCoolTime <= 0)
        {
            _coolTimeText.text = "";
            return;
        }
        _coolTimeText.text = CurCoolTime.ToString("F0");
    }

    private void ChangeStack(int stack)
    {
        if(stack == -1)
        {
            _stackText.text = "";
            return;
        }
        _stackText.text = stack.ToString();
    }
}
