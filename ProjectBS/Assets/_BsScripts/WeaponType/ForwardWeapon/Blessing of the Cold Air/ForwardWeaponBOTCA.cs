using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Yeon;

public class ForwardWeaponBOTCA : Bless
{
    public Transform clonesParent; // ������ ������� ������ ��
    public Transform myTarget; // ���� Ÿ��
    public Transform myRotate; // ���� ȸ���� Ÿ��
    public GameObject weaponPrefab; // ������ ������

    public float ReTime { get => _reTime; set => _reTime = value; }
    public float WaitTime { get => _waitTime; set => _waitTime = value; }
    public float DestroyTime { get => _destroyTime; set => _destroyTime = value; }

    [SerializeField] private float _reTime; // ���ݼӵ�
    [SerializeField] private float _waitTime; // ���� ������ ������ð�
    [SerializeField] private float _destroyTime; // ������ ���� ���� �ð�

    float time = 0.0f;
    short Level = 0;
    short Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Level = Count = 0;
        if (myTarget != null) transform.SetParent(myTarget); // �÷��̾ ����
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = myRotate.rotation; // ȸ�� ����
        time += Time.deltaTime;
        SwitchUpdate();
        
    }

    public void OnOkSpawnForwardWeapon() // Ŭ���� ȣ��
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
                if (Count < 1)
                {
                    Count++;
                    Amount++;
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;
            case 2:  //  2���� �ȴ�.
                if (Count < 1)
                {
                    Count++;
                    Amount++;
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;
            case 3:  //  ����� 20% ����
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 20% ����");
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;
            case 4:  //  3���� �ȴ�.
                if (Count < 1)
                {
                    Count++;
                    Amount++;
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;
            case 5:  //  ���ݼӵ� 50% ����
                if (Count < 1)
                {
                    Count++;
                    ReTime -= ReTime * 0.5f;
                    Debug.Log("���ݼӵ� 50% ����");
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;
            case 6:  //  4���� �ȴ�.
                if (Count < 1)
                {
                    Count++;
                    Amount++;
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;
            case 7:  //  5���� �ȴ�.
                if (Count < 1)
                {
                    Count++;
                    Amount++;
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;

        }
    }

    private void SpawnWeapon()
    {
        GameObject bullet = Instantiate(weaponPrefab, transform.position, transform.rotation); // ���� ����
        bullet.transform.localScale = new Vector3(Size, Size, Size);
        bullet.transform.SetParent(clonesParent); // ������ ���� ��ó��
        Destroy(bullet, DestroyTime);
    }
     IEnumerator SpawnMultipleWeapons(int v)
    {
        for (int i = 0; i < v; i++)
        {
            SpawnWeapon();
            yield return new WaitForSeconds(WaitTime);
        }
    }

}