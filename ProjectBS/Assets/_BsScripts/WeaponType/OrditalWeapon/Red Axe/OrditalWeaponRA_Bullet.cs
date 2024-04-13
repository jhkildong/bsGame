using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class OrditalWeaponRA_Bullet : Bless
{
    public LayerMask Monster;
    public float rotSpeed = 1000.0f; // 회전속도

    private void OnTriggerEnter(Collider other) // 대미지
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
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime, Space.World);// 회전
    }
}
