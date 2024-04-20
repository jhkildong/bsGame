using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class RangeWeaponBOL : Bless
{
    public Transform clonesParent; // 생성한 프리펩들 보관할 곳
    public Transform myTarget; // 따라갈 타겟
    public GameObject weaponPrefab; // 생성할 프리팹

    public float ReTime { get => _reTime; set => _reTime = value; }
    public float WaitTime { get => _waitTime; set => _waitTime = value; }
    public float DestroyTime { get => _destroyTime; set => _destroyTime = value; }
    public float AtRange { get => _atRange; set => _atRange = value; }

    [SerializeField] private float _reTime; // 공격속도
    [SerializeField] private float _waitTime; // 재생성 시간
    [SerializeField] private float _destroyTime; // 생성한 무기 없는 시간
    [SerializeField] private float _atRange; // 플레이어로부터 무기 생성거리

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
        time += Time.deltaTime;
        SwitchUpdate();
    }

    public void OnOkSpawnRangeWaepon()
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
            case 2:  //  2개가 된다.
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
            case 3:  //  대미지 20% 증가.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 30% 증가");
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
            case 5:  //  지속시간이 50% 증가.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("지속시간이 50% 증가");
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;
            case 6:  //  대미지 30% 증가.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 30% 증가");
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(Amount));
                }
                break;
            case 7:  //  4개가 된다.
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
        Vector3 randomPos = Random.insideUnitSphere * AtRange;
        Vector3 spawnPos = myTarget.position + randomPos;
        spawnPos.y = 0.0f;
        GameObject bullet = Instantiate(weaponPrefab, spawnPos, Quaternion.identity); // 무기 생성
        bullet.transform.SetParent(clonesParent);
        Destroy(bullet, DestroyTime);
    }

    IEnumerator SpawnMultipleWeapons(int v)
    {
        for (int i = 0; i < v; i++)
        {
            SpawnWeapon();
        }
        yield return new WaitForSeconds(WaitTime);
    }

}


