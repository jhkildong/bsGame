using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponRM_Bullet : MonoBehaviour
{
    public LayerMask Monster;
    public float Ak;
    public float DelayTime;
    public float AtRange;

    float time = 0.0f;

    private void OnTriggerStay(Collider other)
    {
        if (time >= DelayTime)
        {
            OnAttacking();
            time = 0.0f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }

    private void OnAttacking() // ´ë¹ÌÁö
    {
        Collider[] list = Physics.OverlapSphere(transform.position, AtRange, Monster);

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
