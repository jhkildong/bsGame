using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneshotItem : Item
{
    public OneshotItemData OneshotItemData { get; private set; }
    public override void Init(ItemData data)
    {
        base.Init(data);
        OneshotItemData = (data as OneshotItemData);
    }
}