using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class OrditalWeaponPF : Bless
{
    public Transform myTarget; // ���� Ÿ��
    public GameObject weaponBowPrefab; // Ȱ ������ ������

    public float ReTime { get => _reTime; set => _reTime = value; }
    public float WaitTime { get => _waitTime; set => _waitTime = value; }
    public float DestroyTime { get => _destroyTime; set => _destroyTime = value; }
    public float AtRange { get => _atRange; set => _atRange = value; }
    public float RotSpeed { get => _rotSpeed; set => _rotSpeed = value; }
    public float BulletSpeed { get => _bulletSpeed; set => _bulletSpeed = value; }
    public short ArrowAmount { get => _arrowAmount; set => _arrowAmount = value; }


    [SerializeField] private float _reTime; // ���ݼӵ�
    [SerializeField] private float _waitTime; // ����� �ð�
    [SerializeField] private float _destroyTime; // ������ ���� ���� �ð�
    [SerializeField] private float _atRange; // �÷��̾�κ��� ���� �����Ÿ�
    [SerializeField] private float _rotSpeed; // �����ӵ�
    [SerializeField] private float _bulletSpeed; // ȭ�� �߻�ü �ӵ�
    [SerializeField] private short _arrowAmount; // ȭ�� ����

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
                    ArrowAmount++;
                    SpawnWeaponBow();
                }
                break;
            case 2:  //  2���� �ȴ�.
                if (Amount < 2)
                {
                    Amount++;
                    ArrowAmount++;
                    SpawnWeaponBow();
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
                    ArrowAmount++;
                    SpawnWeaponBow();
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
                    ArrowAmount++;
                    SpawnWeaponBow();
                }
                break;

        }
    }


    private void SpawnWeaponBow() // Ȱ ����
    {
        GameObject bullet = Instantiate(weaponBowPrefab, transform); // ���� ����
        // ������ ���� ������ �����ϰ� ����.
        int childCount = transform.childCount;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
            child.position = transform.position + direction * AtRange;
        }
    }
}