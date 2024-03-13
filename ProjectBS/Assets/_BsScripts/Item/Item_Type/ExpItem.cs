using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItem : Item
{
    
    // Start is called before the first frame update
    public ExpItemData ExpItemData { get; private set; }

    public override void Init(ItemData data)
    {
        base.Init(data);
        ExpItemData = (data as ExpItemData);
    }
}
