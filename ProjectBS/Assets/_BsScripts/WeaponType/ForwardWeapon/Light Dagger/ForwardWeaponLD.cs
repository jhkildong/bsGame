using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using Yeon;

public class ForwardWeaponLD : Bless
{
    public static Quaternion myRotation; // 회전값

    public Transform myTarget; // 따라갈 타겟
    public Transform myRotate; // 따라서 회전할 타겟
    public GameObject weaponPrefab; // 생성할 프리팹
    public Transform clonesParent; // 생성한 프리펩들 보관할 곳

    public float ReTime { get => _reTime; set => _reTime = value; }
    public float WaitTime { get => _waitTime; set => _waitTime = value; }
    public float DestroyTime { get => _destroyTime; set => _destroyTime = value; }

    [SerializeField] private float _reTime; // 공격속도
    [SerializeField] private float _waitTime; // 다음 프리펩 재생성시간
    [SerializeField] private float _destroyTime; // 생성한 무기 없는 시간

    float time = 0.0f; // 시간 저장 변수
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
        myRotation = transform.rotation = myRotate.rotation; // 회전 맞춤
        time += Time.deltaTime;
        SwitchUpdate();
        
    }

    public void OnOkSpawnForwardWeapon()
    {
        if(Level < 7)
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
            case 2:  //  대미지 20% 증가.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 20% 증가");
                }
                if (time >= ReTime)
                {
                    time = 0.0f;
                    SpawnWeapon();
                }
                break;
            case 3:  //  2개가 된다.
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
            case 4:  //  대미지 30% 증가.
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
            case 5:  //  3개가 된다.
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
            case 6:  //  공격속도 30% 증가.
                if (Count < 1)
                {
                    Count++;
                    ReTime -= ReTime * 0.3f;
                    Debug.Log("공격속도 30% 증가.");
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
        GameObject bulletLD = Instantiate(weaponPrefab, transform); // 무기 생성
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            child.localScale = new Vector3(Size, Size, Size);
        }
        bulletLD.transform.SetParent(clonesParent); // 생성 무기 똥처리
        Destroy(bulletLD, DestroyTime);
    }
}