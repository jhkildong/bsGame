using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_Oneshot_", menuName = "Item/Oneshot_Item", order = 6)]
public class OneshotItemData : ItemData
{
    public bool used => _used;

    [SerializeField] private bool _used;
    public override Item CreateItem()
    {
        GameObject itemObject = new GameObject(Name);
        itemObject.AddComponent<OneshotItem>();
        return itemObject.GetComponent<OneshotItem>();
    }
}
