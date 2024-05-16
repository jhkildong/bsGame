using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetItem : Item
{
    public override void Init(ItemData data)
    {
        base.Init(data);
        //ExpItemData = (data as GoldItemData);
    }

    protected override void Eat()
    {
        base.Eat();
        ItemManager.Instance.EatAllItem();
    }
}
