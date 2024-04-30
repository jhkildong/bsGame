using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMovingWeapon : Weapon
{
    private Rigidbody rb;
    private ParticleSystem ps;

    // Start is called before the first frame update
    private void Awake()
    {
        if(!TryGetComponent(out rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        if(!TryGetComponent(out ps))
        {
            ps = gameObject.AddComponent<ParticleSystem>();
        }
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnEnable()
    {
        StartCoroutine(DelayRelease(5.0f));
    }

    public void Shoot(float speed)
    {
        rb.AddForce(transform.forward * speed, ForceMode.VelocityChange);
    }

    IEnumerator DelayRelease(float time)
    {
        yield return new WaitForSeconds(time);
        rb.velocity = Vector3.zero;
        ObjectPoolManager.Instance.ReleaseObj(this);
    }

}
