using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ForwardWeaponLD : MonoBehaviour
{
    public Transform clonesParent; // ������ ������� ������ ��
    public Transform myTarget; // ���� Ÿ��
    public Transform myRotate; // ���� ȸ���� Ÿ��
    public GameObject weaponPrefab; // ������ ������

    public float reTime = 2.0f; // ���ݼӵ�
    public float waitTime = 0.05f; // �� ���� ����

    float time = 0.0f; // �ð� ���� ����

    public short Level = 0;
    short weaponCount = 0; // ���� ����
    short Count = 0;
    // Start is called before the first frame update
    void Start()
    {
        Level = weaponCount = Count =  0;
        if (myTarget != null) transform.SetParent(myTarget);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = myRotate.rotation;
        time += Time.deltaTime;
        SwitchUpdate();
        
    }

    public void OnOkSpawnForwardWeapon()
    {
        if(Level < 7)
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
                if (time >= reTime)
                {
                    time = 0.0f;
                    SpawnWeapon();
                }
                break;
            case 2:  //  ����� 20% ����.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 20% ����");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    SpawnWeapon();
                }
                break;
            case 3:  //  2���� �ȴ�.
                if (Count < 2)
                {
                    Count++;
                    weaponCount++;
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 4:  //  ����� 30% ����.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 30% ����");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 5:  //  3���� �ȴ�.
                if (Count < 1)
                {
                    Count++;
                    weaponCount++;
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 6:  //  ���ݼӵ� 30% ����.
                if (Count < 1)
                {
                    Count++;
                    reTime -= reTime * 0.3f;
                    Debug.Log("���ݼӵ� 30% ����.");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 7:  //  4���� �ȴ�.
                if (Count < 1)
                {
                    Count++;
                    weaponCount++;
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;

        }
    }
     IEnumerator SpawnMultipleWeapons(int v)
    {
        for (int i = 0; i < v; i++)
        {
            SpawnWeapon();
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnWeapon()
    {
        GameObject bulletLD = Instantiate(weaponPrefab, transform.position, transform.rotation);
        bulletLD.transform.SetParent(clonesParent);
        Rigidbody SwordRigidbody = bulletLD.GetComponent<Rigidbody>();
    }
}