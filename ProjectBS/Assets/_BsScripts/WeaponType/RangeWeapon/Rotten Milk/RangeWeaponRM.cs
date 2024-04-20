using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class RangeWeaponRM : Bless
{
    public Transform myTarget; // ���� Ÿ��
    public GameObject objectPrefab; // ������ ������
    public Color GizmosColor = Color.black;

    public float DelayTime { get => _delayTime; set => _delayTime = value; }
    public float AtRange { get => _atRange; set => _atRange = value; }

    [SerializeField] private float _delayTime; // �� ���ݽð�
    [SerializeField] private float _atRange; // ���ݹ���

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
        SwitchUpdate();
    }

    public void OnOkSpawnRangeWaepon()
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
            case 2:  //  ���� ������ 20% ����
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("���� ������ 20% ����");
                }
                break;
            case 3:  //  ����� 20% ����.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 20% ����");
                }
                break;
            case 4:  //  ���� ������ 20% ����
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 20% ����");
                }
                break;
            case 5:  //  ���� �����̰� 30% ����.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("���� �����̰� 30% ����.");
                }
                break;
            case 6:  //  ����� 30% ����.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 30% ����");
                }
                break;
            case 7:  //  ���� �ӵ� 40% ����
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("���� �ӵ� 40% ����");
                }
                break;

        }
    }

    private void SpawnWeapon()
    {
        Instantiate(objectPrefab, transform); // ���� ����
    }
}
