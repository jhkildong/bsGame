using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponLP_Bullet : MonoBehaviour
{
    public LayerMask Monster;

    float delayTime; // 다음 공격 까지 시간
    float Ak;

    float inTime = 0.0f;

    private void OnEnable()
    {
        RangeWeaponLP rangeWeaponLP = FindObjectOfType<RangeWeaponLP>();
        if (rangeWeaponLP != null)
        {
            Ak = rangeWeaponLP.Ak;
            delayTime = rangeWeaponLP.DelayTime;
        }
    }

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
                    obj.TakeDamage(Ak);
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
