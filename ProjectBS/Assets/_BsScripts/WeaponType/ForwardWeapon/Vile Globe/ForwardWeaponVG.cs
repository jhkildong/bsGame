using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;
using Yeon;

public class ForwardWeaponVG : Bless
{
    public Transform myTarget; // 따라갈 타겟
    public Transform myRotate; // 따라서 회전할 타겟
    public GameObject weaponPrefab; // 생성할 프리팹

    public float ReTime { get => _reTime; set => _reTime = value; }
    public float WaitTime { get => _waitTime; set => _waitTime = value; }
    public float DestroyTime { get => _destroyTime; set => _destroyTime = value; }
    public float AtRange { get => _atRange; set => _atRange = value; }
    public float MaxRange { get => _maxRange; set => _maxRange = value; }
    public float MinRange { get => _minRange; set => _minRange = value; }

    [SerializeField] private float _reTime; // 공격속도
    [SerializeField] private float _waitTime; // 다음 프리펩 재생성시간
    [SerializeField] private float _destroyTime; // 생성한 무기 없는 시간
    [SerializeField] private float _atRange; // 플레이어로부터 무기 생성거리
    [SerializeField] private float _maxRange; // 최대 폭
    [SerializeField] private float _minRange; // 최소 폭

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
            Debug.Log($"{Level}Level 입니다.");
        }
    }

    void SwitchUpdate()
    {
        switch (Level)
        {
            case 1:  //  1개 생성
                if (time >= ReTime)
                {
                    time = 0.0f;
                    SpawnWeapon();
                }
                break;
            case 2:  //  2개가 된다.
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
            case 3:  //  대미지 20% 증가
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 20% 증가");
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;
            case 4:  //  3개가 된다.
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
            case 5:  //  공격속도 50% 증가
                if (Count < 1)
                {
                    Count++;
                    ReTime -= ReTime * 0.5f;
                    Debug.Log("공격속도 50% 증가");
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;
            case 6:  //  4개가 된다.
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
            case 7:  //  5개가 된다.
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

        GameObject bulletVG = Instantiate(weaponPrefab, transform); // 무기 생성
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            Vector3 direction = Quaternion.Euler(0.0f, 0.0f, 0.0f) * transform.forward;
            float randomXPos = Random.Range(MinRange, MaxRange); // 랜덤 최소, 최대 폭 설정
            child.position = transform.position + new Vector3(randomXPos, 0.0f, 0.0f) + direction * AtRange;

            child.localScale = new Vector3(Size, Size, Size);
        }
        bulletVG.transform.SetParent(null);
        Destroy(bulletVG, DestroyTime);

    }
}