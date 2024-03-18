using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemData Data { get; private set; }


    public virtual void Init(ItemData data)
    {
        //GameObject go= data.CreateItem();
        Instantiate(data.ItemPrefab, transform);
    }
}
