using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OrditalWeaponPFspawn : MonoBehaviour
{
    public GameObject weaponPrefab; // ������ ������

    public float reTime  = 1.0f;
    float time = 0;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Quaternion rotation = ForwardWeaponLD.myRotation;
        transform.rotation = rotation;
        if (time >= reTime)
        {
            time = 0.0f;
            SpawnWeapon();
        }
    }

    private void SpawnWeapon()
    {
        GameObject bulletPF = Instantiate(weaponPrefab, transform); // ���� ����
        bulletPF.transform.SetParent(null); // ���� ���� ��ó��
    }
}
