using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heart : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<Player>().ReceiveHeal(10);
    }
}
