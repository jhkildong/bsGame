using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Struct_", menuName = "Item/Struct_Item", order = 6)]
public class StructItemData : ItemData
{
    public itemtype TypeOfItem = itemtype.structure;
    public enum struct_mat { wood, stone, steel}
    public struct_mat TypeOfMaterial;


    public override Item CreateItem()
    {
        GameObject itemObject = new GameObject(Name);
        itemObject.AddComponent<StructItem>();
        return itemObject.GetComponent<StructItem>();
    }
}
