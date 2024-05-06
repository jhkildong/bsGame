using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Gold_", menuName = "Item/Gold_Item", order = 6)]
public class GoldItemData : ItemData
{
    public itemtype itemType = itemtype.gold;

    // Start is called before the first frame update
    public override Item CreateItem()
    {
        GameObject go = new GameObject(Name);
        GoldItem clone = go.AddComponent<GoldItem>();
        clone.Init(this);

        return clone;
    }
}
