using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OrditalWeaponPF_Bow : MonoBehaviour
{
    public GameObject weaponArrowPrefab; // ������ ������

    float reTime; // ���ݼӵ�
    float waitTime; // �� �߻� �ð�
    float destroyTime; // ������ ���� ���� �ð�
    short arrowAmount; // ȭ�� ����
    float size;

    float time = 0;

    private void OnEnable()
    {
        OrditalWeaponPF orditalWeaponPF = FindObjectOfType<OrditalWeaponPF>();
        if (orditalWeaponPF != null)
        {
            reTime = orditalWeaponPF.ReTime;
            waitTime = orditalWeaponPF.WaitTime;
            destroyTime = orditalWeaponPF.DestroyTime;
            arrowAmount = orditalWeaponPF.ArrowAmount;
            size = orditalWeaponPF.Size;
        }
    }

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
            StartCoroutine(SpawnMultipleWeapons(arrowAmount));
        }
    }

    private void SpawnWeaponArrow() // ȭ�� ����
    {
        GameObject bullet = Instantiate(weaponArrowPrefab, transform.position, transform.rotation); // ���� ����
        bullet.transform.localScale = new Vector3(size, size, size);
        bullet.transform.SetParent(null); // ���� ���� ��ó��
        Destroy(bullet, destroyTime);
    }
    IEnumerator SpawnMultipleWeapons(int v)
    {
        for (int i = 0; i < v; i++)
        {
            SpawnWeaponArrow();
            yield return new WaitForSeconds(waitTime);
        }
    }
}
