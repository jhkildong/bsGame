using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneshotItem : Item
{
    public OneshotItemData OneshotItemData { get; private set; }

    public OneshotItem(OneshotItemData data) : base(data)
    {
        OneshotItemData = data;
    }
}
