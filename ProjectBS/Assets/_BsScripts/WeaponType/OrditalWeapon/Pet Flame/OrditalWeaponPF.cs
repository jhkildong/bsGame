using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class OrditalWeaponPF : Bless
{
    public Transform myTarget; // 따라갈 타겟
    public GameObject weaponBowPrefab; // 활 생성한 프리펩

    public float ReTime { get => _reTime; set => _reTime = value; }
    public float WaitTime { get => _waitTime; set => _waitTime = value; }
    public float DestroyTime { get => _destroyTime; set => _destroyTime = value; }
    public float AtRange { get => _atRange; set => _atRange = value; }
    public float RotSpeed { get => _rotSpeed; set => _rotSpeed = value; }
    public float BulletSpeed { get => _bulletSpeed; set => _bulletSpeed = value; }
    public short ArrowAmount { get => _arrowAmount; set => _arrowAmount = value; }


    [SerializeField] private float _reTime; // 공격속도
    [SerializeField] private float _waitTime; // 재생성 시간
    [SerializeField] private float _destroyTime; // 생성한 무기 없는 시간
    [SerializeField] private float _atRange; // 플레이어로부터 무기 생성거리
    [SerializeField] private float _rotSpeed; // 공전속도
    [SerializeField] private float _bulletSpeed; // 화살 발사체 속도
    [SerializeField] private short _arrowAmount; // 화살 갯수

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
        transform.Rotate(Vector3.up, -RotSpeed * Time.deltaTime); // 공전
        SwitchUpdate();
    }

    public void OnOkSpawnOrditalWeapon()
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
                if (Amount < 1)
                {
                    Amount++;
                    ArrowAmount++;
                    SpawnWeaponBow();
                }
                break;
            case 2:  //  2개가 된다.
                if (Amount < 2)
                {
                    Amount++;
                    ArrowAmount++;
                    SpawnWeaponBow();
                }
                break;
            case 3:  //  대미지 20% 증가.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 20% 증가");
                }
                break;
            case 4:  //  3개가 된다.
                if (Amount < 3)
                {
                    Amount++;
                    ArrowAmount++;
                    SpawnWeaponBow();
                }
                break;
            case 5:  //  회전속도 50% 증가.
                if (Count < 1)
                {
                    Count++;
                    RotSpeed += RotSpeed * 0.5f;
                    Debug.Log("회전속도 50% 증가");
                }
                break;
            case 6:  //  대미지 20% 증가.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 20% 증가");
                }
                break;
            case 7:  //  4개가 된다.
                if (Amount < 4)
                {
                    Amount++;
                    ArrowAmount++;
                    SpawnWeaponBow();
                }
                break;

        }
    }


    private void SpawnWeaponBow() // 활 생성
    {
        GameObject bullet = Instantiate(weaponBowPrefab, transform); // 무기 생성
        // 생성한 무기 간격을 일정하게 맞춤.
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