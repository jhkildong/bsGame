using UnityEngine;

[RequireComponent(typeof(increaseStruct))]
public class StructItem : Item
{
    public StructItemData StructItemData { get; private set; }
    public override void Init(ItemData data)
    {
        base.Init(data);
        StructItemData = (data as StructItemData);
    }
}
