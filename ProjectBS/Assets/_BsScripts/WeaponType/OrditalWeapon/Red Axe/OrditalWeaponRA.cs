using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class OrditalWeaponRA : MonoBehaviour
{
    public Transform myTarget; // ȸ�� �߽���
    public GameObject weaponPrefabs;
    public float rotSpeed = 30.0f; // ���� �ӵ�
    public float attakRange = 1.0f;

    public short Level = 0;
    short weaponCount = 0;
    short Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Level = weaponCount = Count = 0;
        if (myTarget != null) transform.SetParent(myTarget);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, rotSpeed * Time.deltaTime); // ����
        SwitchUpdate();
    }

    public void OnOkSpawnOrditalWeapon()
    {
        if (Level < 7)
        {
            Level++;
            Count = 0;
            Debug.Log($"{Level}Level �Դϴ�.");
        }
    }

    void SwitchUpdate()
    {
        switch (Level)
        {
            case 1:  //  1�� ����
                if (weaponCount < 1)
                {
                    weaponCount++;
                    SpawnWeapon();
                }
                break;
            case 2:  //  2���� �ȴ�.
                if (weaponCount < 2)
                {
                    weaponCount++;
                    SpawnWeapon();
                }
                break;
            case 3:  //  ����� 50% ����.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 50% ����");
                }
                break;
            case 4:  //  3���� �ȴ�.
                if(weaponCount < 3)
                {
                    weaponCount++;
                    SpawnWeapon();
                }
                break;
            case 5:  //  ������ 30% ����.
                if (Count < 1)
                {
                    Count++;
                    attakRange += attakRange * 0.3f;
                    Debug.Log("������ 30% ����");
                }
                break;
            case 6:  //  4���� �ȴ�.
                if (weaponCount < 4)
                {
                    weaponCount++;
                    SpawnWeapon();
                }
                break;
            case 7:  //  5���� �ȴ�.
                if (weaponCount < 5)
                {
                    weaponCount++;
                    SpawnWeapon();
                }
                break;

        }
    }


    private void SpawnWeapon()
    {
        GameObject bulletRA = Instantiate(weaponPrefabs, transform);

        int childCount = transform.childCount;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Vector3 eulerAngle = child.localRotation.eulerAngles;
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
            child.position = transform.position + direction * attakRange;
            child.localRotation = Quaternion.Euler(eulerAngle.x, angleStep * i, eulerAngle.z);
        }
    }
}
