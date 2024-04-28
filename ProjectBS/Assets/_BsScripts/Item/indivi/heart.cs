using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("1");
        if((int)BSLayerMasks.MagneticField != other.gameObject.layer)
        {
            GameObject.Find("Player").GetComponent<Player>().ReceiveHealEffect(10);
        }
    }
}