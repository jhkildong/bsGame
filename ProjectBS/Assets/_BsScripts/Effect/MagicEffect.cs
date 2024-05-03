using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MagicEffect : PlayerAttackType
{
    private Rigidbody myRb;
    private PlayerAttackType hitEffect;

    protected override void Awake()
    {
        base.Awake();
        StringBuilder sb = new StringBuilder(FilePath.MagicHitTypes);
        sb.Append("/MagicHit_");
        sb.Append(ID + 100);
        string path = sb.ToString();
        hitEffect = Resources.Load<PlayerAttackType>(path);
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
        if (isStopped)
            return;
        ObjectPoolManager.Instance.GetEffect(hitEffect, Attack, Size).
            This.transform.SetPositionAndRotation(transform.position, Quaternion.identity);
        isStopped = true;
    }
}
