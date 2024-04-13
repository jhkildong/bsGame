using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class RangeWeaponRM_Bullet : Bless
{
    public LayerMask Monster;

    public float delayTime = 1.0f;
    public float atRange = 2.0f;
    float inTime = 0.0f;

    
    private void OnTriggerStay(Collider other)
    {
        if (inTime >= delayTime)
        {
            OnAttacking();
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
                obj.TakeDamage((short)Mathf.Round(Ak));
                inTime = 0.0f;
            }
        }
    }
}
