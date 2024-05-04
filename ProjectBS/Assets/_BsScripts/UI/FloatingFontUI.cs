using UnityEngine;
using TMPro;

public class FloatingFontUI : FollowingTargetUI
{
    [SerializeField]private TextMeshProUGUI text;

    public void SetDamageUI(int dmg, Vector3 pos)
    {
        text.text = dmg.ToString("N0");
        currentPos = pos;
    }


    public void Release()
    {
        UIManager.Instance.ReleaseUI(this);
    }
}
