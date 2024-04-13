using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class ForwardWeaponBOTCA_Bullet : Bless
{
    public LayerMask Monster;
    public float bulletSpeed = 5.0f; // 이동 속도

    private void OnTriggerEnter(Collider other) // 대미지
    {
        if ((Monster & 1 << other.gameObject.layer) != 0)
        {
            IDamage<Monster> obj = other.GetComponent<IDamage<Monster>>();
            if (obj != null)
            {
                obj.TakeDamage((short)Mathf.Round(Ak));
                Destroy(gameObject);
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
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime); // 이동
    }
}
