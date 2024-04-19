using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerA : MonoBehaviour
{
    void cb(ItemFollow itemFollow)
    {
        itemFollow.follow(this);
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((int)(BSLayerMasks.Item) == (1 << other.gameObject.layer))
        {
            cb(other.GetComponent<ItemFollow>());
        }
    }
}
