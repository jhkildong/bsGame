using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Item_Exp_", menuName = "Item/Exp_Item", order = 5)]
public class ExpItemData : ItemData
{
    //itemtype expitem = itemtype.exp;
    //ù�ٿ��� exp ���������� ����
    public float exprate => _exprate;

    [SerializeField] private float _exprate;
    // Start is called before the first frame update
    public override Item CreateItem()
    {
        GameObject itemObject = new GameObject(Name);
        itemObject.AddComponent<ExpItem>().Init(this);

        return itemObject.GetComponent<ExpItem>();
    }
}
