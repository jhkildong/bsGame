using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Magnet_", menuName = "Item/Magnet_Item", order = 10)]
public class MagnetItemData : OneshotItemData
{
    public override Item CreateItem()
    {
        GameObject go = new GameObject(Name);
        MagnetItem clone = go.AddComponent<MagnetItem>();
        clone.Init(this);

        return clone;
    }
}
