using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : MonoBehaviour
{
    LayerMask Magnetic;
    private void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if(16 != other.gameObject.layer)
        {
            GameObject.Find("Player").GetComponent<Player>().ReceiveHeal(10);
            //Destroy(gameObject);
        }
    }
}
