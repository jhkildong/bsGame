using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMovingWeapon : Weapon
{
    private Rigidbody rb;

    // Start is called before the first frame update
    private void Awake()
    {
        if(!TryGetComponent(out rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    public void Shoot(float speed)
    {
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }

}
