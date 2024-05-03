using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardMovingWeapon : Weapon
{
    private Rigidbody rb;
    private TrailRenderer tr;
    [SerializeField]private float ReleaseTime = 1.5f;

    // Start is called before the first frame update
    private void Awake()
    {
        if(!TryGetComponent(out rb))
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }
        if(!TryGetComponent(out tr))
        {
            tr = gameObject.GetComponentInChildren<TrailRenderer>();
        }
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
    }

    private void OnEnable()
    {
        StartCoroutine(DelayRelease(ReleaseTime));
    }

    public void Shoot(float speed)
    {
        rb.AddForce(transform.forward * speed, ForceMode.Impulse);
    }

    IEnumerator DelayRelease(float time)
    {
        yield return new WaitForEndOfFrame();
        if(tr != null)
            tr.enabled = true;
        yield return new WaitForSeconds(time);
        rb.velocity = Vector3.zero;
        if(tr != null)
        {
            tr.Clear();
            tr.enabled = false;
        }
        ObjectPoolManager.Instance.ReleaseObj(this);
    }

}
