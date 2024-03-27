using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardWeaponLD_Bullet : Weapon
{
    public float bulletSpeed = 5.0f; // 이동 속도

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);
    }
}
