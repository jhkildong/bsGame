using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEffect : Effect
{
    Rigidbody rb;
    private void OnEnable()
    {
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 20, ForceMode.Impulse);
    }

    private void OnDisable()
    {
        rb.velocity = Vector3.zero;
    }
}
