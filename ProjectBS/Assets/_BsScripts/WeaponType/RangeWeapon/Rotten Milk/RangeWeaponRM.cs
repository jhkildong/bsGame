using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponRM : MonoBehaviour
{
    public Transform myTarget;
    public GameObject objectPrefab;

    public short Level = 0;
    short weaponCount = 0; // ���� ����
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
                if (weaponCount < 1)
                {
                    weaponCount++;
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
        GameObject bulletRM = Instantiate(objectPrefab, transform);
        bulletRM.transform.SetParent(myTarget);
    }
}
