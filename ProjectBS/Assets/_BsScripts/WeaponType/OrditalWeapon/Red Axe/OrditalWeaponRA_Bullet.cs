using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrditalWeaponRA_Bullet : Weapon
{
    public float rotSpeed = 1000.0f;

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime, Space.World);
    }
}
