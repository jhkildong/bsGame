using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using Yeon;

public class OrditalWeaponBOTS : Bless
{
    public Transform myTarget; // ���� Ÿ��
    public GameObject weaponPrefab; // ������ ������

    public float AtRange { get => _atRange; set => _atRange = value; }
    public float RotSpeed { get => _rotSpeed; set => _rotSpeed = value; }

    [SerializeField] private float _rotSpeed; // �÷��̾�κ��� ���� �����Ÿ�
    [SerializeField] private float _atRange; // �÷��̾�κ��� ���� �����Ÿ�

    short Level = 0;
    short Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Level = Count = 0;
        if (myTarget != null) transform.SetParent(myTarget);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, -RotSpeed * Time.deltaTime); // ����
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
                if (Amount < 1)
                {
                    Amount++;
                    SpawnWeapon();
                }
                break;
            case 2:  //  2���� �ȴ�.
                if (Amount < 2)
                {
                    Amount++;
                    SpawnWeapon();
                }
                break;
            case 3:  //  ����� 20% ����.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 20% ����");
                }
                break;
            case 4:  //  3���� �ȴ�.
                if (Amount < 3)
                {
                    Amount++;
                    SpawnWeapon();
                }
                break;
            case 5:  //  ȸ���ӵ� 50% ����.
                if (Count < 1)
                {
                    Count++;
                    RotSpeed += RotSpeed * 0.5f;
                    Debug.Log("ȸ���ӵ� 50% ����");
                }
                break;
            case 6:  //  ����� 20% ����.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 20% ����");
                }
                break;
            case 7:  //  4���� �ȴ�.
                if (Amount < 4)
                {
                    Amount++;
                    SpawnWeapon();
                }
                break;

        }
    }


    private void SpawnWeapon()
    {
        GameObject bullet = Instantiate(weaponPrefab, transform); // ���� ����
        bullet.transform.localScale = new Vector3(Size, Size, Size);

        // ������ ���� ������ �����ϰ� ����.
        int childCount = transform.childCount;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Vector3 eulerAngle = child.localRotation.eulerAngles;
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
            child.position = transform.position + direction * AtRange;
            child.localRotation = Quaternion.Euler(eulerAngle.x, angleStep * i, eulerAngle.z); // ������ rotation�� Ÿ�������� ����.

        }
    }
}
