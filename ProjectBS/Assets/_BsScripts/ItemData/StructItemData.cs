using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item_Struct_", menuName = "Item/Struct_Item", order = 6)]
public class StructItemData : ItemData
{
    
    //public enum matType => _matType;

    //[SerializeField] private enum _matType;

    public override Item CreateItem()
    {
        return new StructItem(this);
    }
}
