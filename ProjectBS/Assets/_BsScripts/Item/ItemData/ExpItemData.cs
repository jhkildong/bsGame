using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_Exp_", menuName = "Item/Exp_Item", order = 5)]
public class ExpItemData : ItemData
{
    public itemtype itemType = itemtype.exp;

    //itemtype expitem = itemtype.exp;
    //첫줄에서 exp 아이템으로 설정
    public float exprate => _exprate;

    [SerializeField] private float _exprate;
    // Start is called before the first frame update
    public override Item CreateItem()
    {
        GameObject go = new GameObject(Name);
        ExpItem clone = go.AddComponent<ExpItem>();
        clone.Init(this);

        return clone;
    }
}
