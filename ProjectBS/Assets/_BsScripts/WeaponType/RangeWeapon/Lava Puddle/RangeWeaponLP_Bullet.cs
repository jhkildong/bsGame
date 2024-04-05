using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class RangeWeaponLP_Bullet : Bless
{
    public LayerMask Monster;

    public float delayTime = 1.0f;
    float inTime = 0.0f;

    void SomeMethod()
    {
        float attack = Ak;
        float size = Size;
        BlessData data = Data;
        LevelProperty levelProp = LevelProp;
    }

    private void OnTriggerStay(Collider other)
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
