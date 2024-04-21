using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicEffect : Effect
{
    private Rigidbody myRb;
    private Effect hitEffect;

    protected override void Awake()
    {
        base.Awake();
        string path = $"Effect/MagicHit/MagicHit_{ID+100}";
        hitEffect = Resources.Load<Effect>(path);
        ObjectPoolManager.Instance.SetPool(hitEffect , 10, 10);
    }

    public void Shoot()
    {
        if (myRb == null)
            myRb = GetComponent<Rigidbody>();
        myRb.AddForce(transform.forward * 10, ForceMode.VelocityChange);
    }

    private void OnDisable()
    {
        if (myRb != null)
            myRb.velocity = Vector3.zero;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        ObjectPoolManager.Instance.GetEffect(hitEffect, attack : Attack).
            This.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        isStopped = true;
    }
}
