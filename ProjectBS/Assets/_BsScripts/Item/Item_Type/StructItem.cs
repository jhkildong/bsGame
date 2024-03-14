using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructItem : Item
{
    public StructItemData StructItemData { get; private set; }
    public override void Init(ItemData data)
    {
        base.Init(data);
        StructItemData = (data as StructItemData);
    }
}
