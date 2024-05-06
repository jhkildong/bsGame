using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldItem : Item
{
    // Start is called before the first frame update
    public GoldItemData ExpItemData
    {
        get; private set;
    }

    public int Gold
    {
        get => _gold;
        set
        {
            _gold = value;
            int step = (int)(value * 0.02f);
            transform.localScale = Vector3.one * ((1.0f + step * 0.3f));
        }
    }
    private int _gold;

    public override void Init(ItemData data)
    {
        base.Init(data);
        ExpItemData = (data as GoldItemData);
    }

    protected override void Eat()
    {
        base.Eat();
        GameManager.Instance.ChangeGold(Gold);
    }
}
