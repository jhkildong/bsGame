using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Coffee.UIEffects;

public class SkillIcon : UIComponent
{
    public Sprite[] skillSprites;

    [SerializeField] private Image _icon;
    [SerializeField] private Image _coolTimeicon;
    [SerializeField] private TextMeshProUGUI _coolTimeText;
    [SerializeField] private TextMeshProUGUI _stackText;
    [SerializeField] private UIShiny uIShiny;
    [SerializeField] private UIShiny skillduringShiny;

    private void Start()
    {
        GameManager.Instance.Player.ChangeCoolTimeAct += ChangeImg;
        GameManager.Instance.Player.ChangeCoolTimeAct += ChangeText;
        GameManager.Instance.Player.ChangeBuffTimeAct += ChangeText;
        GameManager.Instance.Player.ChangeStackAct += ChangeStack;

        ChangeImg(0, 1);
        ChangeText(0, 1);
        ChangeStack(-1);
        uIShiny.Play();
    }
    public void SetIcon(int idx)
    {
        _icon.sprite = skillSprites[idx];
        _coolTimeicon.sprite = skillSprites[idx];
    }

    public void SetSkillDuring(bool state)
    {
        if (state)
        {
            skillduringShiny.gameObject.SetActive(true);
            skillduringShiny.Play();
            uIShiny.Stop();
        }
        else
        {
            skillduringShiny.gameObject.SetActive(false);
            skillduringShiny.Stop();
        }
    }
    public void SetTextColor(Color color)
    {
        _coolTimeText.color = color;
    }

    private void ChangeImg(float CurCoolTime, float MaxCoolTime)
    {
        if(CurCoolTime <= 0)
        {
            _coolTimeicon.fillAmount = 0;
            return;
        }
        _coolTimeicon.fillAmount = CurCoolTime / MaxCoolTime;
        if (_coolTimeicon.fillAmount >= 1)
            uIShiny.Play();
        else
            uIShiny.Stop();
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
