using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ItemFollow))]
[RequireComponent(typeof(SphereCollider))]
public abstract class Item : MonoBehaviour
{
    public ItemData Data { get; private set; }


    public virtual void Init(ItemData data)
    {
        Instantiate(data.ItemPrefab, transform);
        gameObject.layer = 17;
    }
}
