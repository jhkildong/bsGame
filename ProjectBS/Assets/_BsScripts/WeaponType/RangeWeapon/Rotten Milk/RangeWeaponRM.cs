using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon;

public class RangeWeaponRM : Bless
{
    public Transform myTarget; // 따라갈 타겟
    public GameObject objectPrefab; // 생성할 프리펩
    public Color GizmosColor = Color.black;

    public float DelayTime { get => _delayTime; set => _delayTime = value; }
    public float AtRange { get => _atRange; set => _atRange = value; }

    [SerializeField] private float _delayTime; // 재 공격시간
    [SerializeField] private float _atRange; // 공격범위

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
                    SpawnWeapon();
                }
                break;
            case 2:  //  공격 범위가 20% 증가
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("공격 범위가 20% 증가");
                }
                break;
            case 3:  //  대미지 20% 증가.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 20% 증가");
                }
                break;
            case 4:  //  공격 범위가 20% 증가
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 20% 증가");
                }
                break;
            case 5:  //  공격 딜레이가 30% 감소.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("공격 딜레이가 30% 감소.");
                }
                break;
            case 6:  //  대미지 30% 증가.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 30% 증가");
                }
                break;
            case 7:  //  공격 속도 40% 증가
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("공격 속도 40% 증가");
                }
                break;

        }
    }

    private void SpawnWeapon()
    {
        Instantiate(objectPrefab, transform); // 무기 생성
    }
}
