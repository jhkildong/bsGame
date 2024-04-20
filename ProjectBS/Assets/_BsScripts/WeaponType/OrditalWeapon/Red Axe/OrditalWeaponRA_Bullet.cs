using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrditalWeaponRA_Bullet : MonoBehaviour
{
    public LayerMask Monster;
    float bulletRotSpeed; // ȸ���ӵ�
    float Ak;

    private void OnEnable()
    {
        OrditalWeaponRA forwardWeaponRA = FindObjectOfType<OrditalWeaponRA>();
        if (forwardWeaponRA != null)
        {
            Ak = forwardWeaponRA.Ak;
            bulletRotSpeed = forwardWeaponRA.BulletRotSpeed;
        }
    }

    private void OnTriggerEnter(Collider other) // �����
    {
        if ((Monster & 1 << other.gameObject.layer) != 0)
        {
            IDamage<Monster> obj = other.GetComponent<IDamage<Monster>>();
            if (obj != null)
            {
                obj.TakeDamage(Ak);
            }
        }
    }

    void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * bulletRotSpeed * Time.deltaTime, Space.World);// ȸ��
    }
}
