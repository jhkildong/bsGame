using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_Oneshot_", menuName = "Item/Oneshot_Item", order = 6)]
public class OneshotItemData : ItemData
{
    public itemtype itemType = itemtype.oneshot;
    public bool used => _used;

    [SerializeField] private bool _used;
    public override Item CreateItem()
    {
        GameObject go = new GameObject(Name);
        OneshotItem clone = go.AddComponent<OneshotItem>();
        clone.Init(this);

        return clone;
    }
}
