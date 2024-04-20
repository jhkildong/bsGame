using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yeon;

public class OrditalWeaponRA : Bless
{
    public Transform myTarget; // ȸ�� �߽���
    public GameObject weaponPrefab; // ���� ������

    public float AtRange { get => _atRange; set => _atRange = value; }
    public float RotSpeed { get => _rotSpeed; set => _rotSpeed = value; }
    public float BulletRotSpeed { get => _bulletRotSpeed; set => _bulletRotSpeed = value; }

    [SerializeField] private float _atRange; // �÷��̾�κ��� ���� �����Ÿ�
    [SerializeField] private float _rotSpeed; // �����ӵ�
    [SerializeField] private float _bulletRotSpeed; // �߻�ü ���� �ӵ�

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
        transform.Rotate(Vector3.up, RotSpeed * Time.deltaTime); // ����
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
            case 3:  //  ����� 50% ����.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 50% ����");
                }
                break;
            case 4:  //  3���� �ȴ�.
                if(Amount < 3)
                {
                    Amount++;
                    SpawnWeapon();
                }
                break;
            case 5:  //  ������ 30% ����.
                if (Count < 1)
                {
                    Count++;
                    AtRange += AtRange * 0.3f;
                    Debug.Log("������ 30% ����");
                }
                break;
            case 6:  //  4���� �ȴ�.
                if (Amount < 4)
                {
                    Amount++;
                    SpawnWeapon();
                }
                break;
            case 7:  //  5���� �ȴ�.
                if (Amount < 5)
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

        // ������ ����� ���� ����.
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
