using UnityEngine;
using TMPro;

public class FloatingFontUI : FollowingTargetUI
{
    public override int ID => _id;
    [SerializeField]private int _id;
    [SerializeField]private TextMeshProUGUI text;

    public void SetDamage(int dmg, Transform target)
    {
        text.text = dmg.ToString("N0");
        currentPos = target.position;
        
    }

    public void Release()
    {
        UIManager.Instance.ReleaseUI(this);
    }
}
