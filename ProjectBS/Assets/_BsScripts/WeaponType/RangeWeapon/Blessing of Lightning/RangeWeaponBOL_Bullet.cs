using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class RangeWeaponBOL_Bullet : Bless
{
    public LayerMask Monster;
    
    private void OnTriggerEnter(Collider other)
    {
        if ((Monster & 1 << other.gameObject.layer) != 0)
        {
            IDamage<Monster> obj = other.GetComponent<IDamage<Monster>>();
            if (obj != null)
            {
                obj.TakeDamage((short)Mathf.Round(Ak));
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
