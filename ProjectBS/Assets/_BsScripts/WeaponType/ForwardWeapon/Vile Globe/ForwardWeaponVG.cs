using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;
using Yeon;

public class ForwardWeaponVG : Bless
{
    public Transform myTarget; // ���� Ÿ��
    public Transform myRotate; // ���� ȸ���� Ÿ��
    public GameObject weaponPrefab; // ������ ������

    public float ReTime { get => _reTime; set => _reTime = value; }
    public float WaitTime { get => _waitTime; set => _waitTime = value; }
    public float DestroyTime { get => _destroyTime; set => _destroyTime = value; }
    public float AtRange { get => _atRange; set => _atRange = value; }
    public float MaxRange { get => _maxRange; set => _maxRange = value; }
    public float MinRange { get => _minRange; set => _minRange = value; }

    [SerializeField] private float _reTime; // ���ݼӵ�
    [SerializeField] private float _waitTime; // ���� ������ ������ð�
    [SerializeField] private float _destroyTime; // ������ ���� ���� �ð�
    [SerializeField] private float _atRange; // �÷��̾�κ��� ���� �����Ÿ�
    [SerializeField] private float _maxRange; // �ִ� ��
    [SerializeField] private float _minRange; // �ּ� ��

    float time = 0.0f;
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
        transform.rotation = myRotate.rotation;
        time += Time.deltaTime;
        SwitchUpdate();
    }

    public void OnOkSpawnForwardWeapon()
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
                if (time >= ReTime)
                {
                    time = 0.0f;
                    SpawnWeapon();
                }
                break;
            case 2:  //  2���� �ȴ�.
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

        GameObject bulletVG = Instantiate(weaponPrefab, transform); // ���� ����
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            Vector3 direction = Quaternion.Euler(0.0f, 0.0f, 0.0f) * transform.forward;
            float randomXPos = Random.Range(MinRange, MaxRange); // ���� �ּ�, �ִ� �� ����
            child.position = transform.position + new Vector3(randomXPos, 0.0f, 0.0f) + direction * AtRange;

            child.localScale = new Vector3(Size, Size, Size);
        }
        bulletVG.transform.SetParent(null);
        Destroy(bulletVG, DestroyTime);

    }
}