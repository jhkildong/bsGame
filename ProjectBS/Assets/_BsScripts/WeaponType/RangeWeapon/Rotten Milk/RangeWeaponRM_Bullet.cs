using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponRM_Bullet : MonoBehaviour
{
    public LayerMask Monster;

    float Ak;
    float delayTime;
    float atRange;

    float inTime = 0.0f;
    private void OnEnable()
    {
        RangeWeaponRM rangeWeaponRM = FindObjectOfType<RangeWeaponRM>();
        if (rangeWeaponRM != null)
        {
            Ak = rangeWeaponRM.Ak;
            delayTime = rangeWeaponRM.DelayTime;
            atRange = rangeWeaponRM.AtRange;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (inTime >= delayTime)
        {
            OnAttacking();
            inTime = 0.0f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inTime += Time.deltaTime;
    }

    private void OnAttacking() // ´ë¹ÌÁö
    {
        Collider[] list = Physics.OverlapSphere(transform.position, atRange, Monster);

        foreach (Collider col in list)
        {
            IDamage<Monster> obj = col.GetComponent<IDamage<Monster>>();
            if (obj != null)
            {
                obj.TakeDamage(Ak);
            }
        }
    }


}
