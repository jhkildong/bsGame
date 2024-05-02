using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mag : MonoBehaviour
{
    float r;

    private void OnTriggerEnter(Collider other)
    {
        r = other.GetComponent<SphereCollider>().radius;
        other.GetComponent<SphereCollider>().radius = 100;
        other.GetComponent<SphereCollider>().radius = r;
    }
}
