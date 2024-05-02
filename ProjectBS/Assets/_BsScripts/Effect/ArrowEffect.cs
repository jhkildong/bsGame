using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowEffect : PlayerAttackType
{
    Rigidbody rb;

    public void Shoot()
    {
        if(rb == null)
            rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * 25, ForceMode.Impulse);
    }

    private void OnDisable()
    {
        if(rb != null)
            rb.velocity = Vector3.zero;
    }
}
