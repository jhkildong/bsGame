using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class RangeWeaponLP_Bullet : Bless
{
    public LayerMask Monster;

    public float delayTime = 1.0f; // 다음 공격 까지 시간
    float inTime = 0.0f;

    
    private void OnTriggerStay(Collider other) // 대미지
    {
        inTime += Time.deltaTime;
        if(inTime >= delayTime)
        {
            if ((Monster & 1 << other.gameObject.layer) != 0)
            {
                IDamage<Monster> obj = other.GetComponent<IDamage<Monster>>();
                if (obj != null)
                {
                    obj.TakeDamage((short)Mathf.Round(Ak));
                    inTime = 0.0f;
                }
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
