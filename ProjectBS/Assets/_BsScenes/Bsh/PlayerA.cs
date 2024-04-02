using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerA : MonoBehaviour
{
    UnityEvent unityEvent;

    void Start()
    {
        if (unityEvent == null)
        {
            unityEvent = new UnityEvent();
        }
    }
    void Ping()
    {
        Debug.Log("Ping");
    }
    void cb(ItemFollow itemFollow)
    {
        itemFollow.test(this);
    }
    void Update()
    {
        //unityEvent.Invoke();
    }
    private void OnTriggerEnter(Collider other)
    {
        //unityEvent.AddListener(other.GetComponent<ItemFollow>().test);
        if ((int)(BSLayerMasks.Item) == (1 << other.gameObject.layer))
        {
            cb(other.GetComponent<ItemFollow>());
        }
    }
}
