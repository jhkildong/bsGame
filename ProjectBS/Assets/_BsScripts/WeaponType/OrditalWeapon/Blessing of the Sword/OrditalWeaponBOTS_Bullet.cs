using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yeon;

public class OrditalWeaponBOTS_Bullet : Bless
{
    public LayerMask Monster;

    void SomeMethod()
    {
        float attack = Ak;
        float size = Size;
        BlessData data = Data;
        LevelProperty levelProp = LevelProp;
    }

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

    void Start()
    {

    }
    void Update()
    {

    }
}
