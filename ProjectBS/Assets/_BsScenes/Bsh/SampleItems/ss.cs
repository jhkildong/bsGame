using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ss : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SphereCollider collider = GetComponent<SphereCollider>();
        collider.radius = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        float dd = other.GetComponentInParent<SphereCollider>().radius;
        Debug.Log(dd);
    }
}
