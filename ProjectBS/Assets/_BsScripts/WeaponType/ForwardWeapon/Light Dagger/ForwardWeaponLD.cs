using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Yeon;

public class ForwardWeaponLD : Bless
{
    public static Quaternion myRotation; // ȸ����

    public Transform myTarget; // ���� Ÿ��
    public Transform myRotate; // ���� ȸ���� Ÿ��
    public GameObject weaponPrefab; // ������ ������
    public Transform clonesParent; // ������ ������� ������ ��

    public float ReTime { get => _reTime; set => _reTime = value; }
    public float WaitTime { get => _waitTime; set => _waitTime = value; }
    public float DestroyTime { get => _destroyTime; set => _destroyTime = value; }

    [SerializeField] private float _reTime; // ���ݼӵ�
    [SerializeField] private float _waitTime; // ���� ������ ������ð�
    [SerializeField] private float _destroyTime; // ������ ���� ���� �ð�

    float time = 0.0f; // �ð� ���� ����
    short Level = 0;
    short Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Level = Count =  0;
        if (myTarget != null) transform.SetParent(myTarget);
    }

    // Update is called once per frame
    void Update()
    {
        myRotation = transform.rotation = myRotate.rotation; // ȸ�� ����
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
                if (time >= ReTime)
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
                if (time >= ReTime)
                {
                    time = 0.0f;
                    SpawnWeapon();
                }
                break;
            case 3:  //  2���� �ȴ�.
                if (Count < 2)
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
            case 4:  //  ����� 30% ����.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 30% ����");
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;
            case 5:  //  3���� �ȴ�.
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
            case 6:  //  ���ݼӵ� 30% ����.
                if (Count < 1)
                {
                    Count++;
                    ReTime -= ReTime * 0.3f;
                    Debug.Log("���ݼӵ� 30% ����.");
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;
            case 7:  //  4���� �ȴ�.
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
     IEnumerator SpawnMultipleWeapons(int v)
    {
        for (int i = 0; i < v; i++)
        {
            SpawnWeapon();
            yield return new WaitForSeconds(WaitTime);
        }
    }

    private void SpawnWeapon()
    {
        GameObject bulletLD = Instantiate(weaponPrefab, transform); // ���� ����
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            child.localScale = new Vector3(Size, Size, Size);
        }
        bulletLD.transform.SetParent(clonesParent); // ���� ���� ��ó��
        Destroy(bulletLD, DestroyTime);
    }
}