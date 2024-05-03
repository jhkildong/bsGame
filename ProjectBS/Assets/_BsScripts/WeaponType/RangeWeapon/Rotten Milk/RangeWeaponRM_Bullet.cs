using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponRM_Bullet : Weapon
{
    public float DelayTime;
    public float AtRange;

    float time = 0.0f;

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        Collider[] list = Physics.OverlapSphere(transform.position, AtRange, Monster);

        if (time >= DelayTime)
        {
            time = 0.0f;
            foreach (Collider col in list)
            {
                IDamage<Monster> obj = col.GetComponent<IDamage<Monster>>();
                if (obj != null)
                {
                    obj.TakeDamage(Ak);
                    Debug.Log("Attack");
                }
            }
        }
    }
}
