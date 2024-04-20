using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Yeon;

public class OrditalWeaponRA : Bless
{
    public Transform myTarget; // 회전 중심점
    public GameObject weaponPrefab; // 무기 프리펩

    public float AtRange { get => _atRange; set => _atRange = value; }
    public float RotSpeed { get => _rotSpeed; set => _rotSpeed = value; }
    public float BulletRotSpeed { get => _bulletRotSpeed; set => _bulletRotSpeed = value; }

    [SerializeField] private float _atRange; // 플레이어로부터 무기 생성거리
    [SerializeField] private float _rotSpeed; // 공전속도
    [SerializeField] private float _bulletRotSpeed; // 발사체 공전 속도

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
        transform.Rotate(Vector3.up, RotSpeed * Time.deltaTime); // 공전
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
                    SpawnWeapon();
                }
                break;
            case 2:  //  2개가 된다.
                if (Amount < 2)
                {
                    Amount++;
                    SpawnWeapon();
                }
                break;
            case 3:  //  대미지 50% 증가.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 50% 증가");
                }
                break;
            case 4:  //  3개가 된다.
                if(Amount < 3)
                {
                    Amount++;
                    SpawnWeapon();
                }
                break;
            case 5:  //  범위가 30% 증가.
                if (Count < 1)
                {
                    Count++;
                    AtRange += AtRange * 0.3f;
                    Debug.Log("범위가 30% 증가");
                }
                break;
            case 6:  //  4개가 된다.
                if (Amount < 4)
                {
                    Amount++;
                    SpawnWeapon();
                }
                break;
            case 7:  //  5개가 된다.
                if (Amount < 5)
                {
                    Amount++;
                    SpawnWeapon();
                }
                break;

        }
    }

    private void SpawnWeapon()
    {
        GameObject bullet = Instantiate(weaponPrefab, transform); // 무기 생성
        bullet.transform.localScale = new Vector3(Size, Size, Size);

        // 생성한 무기들 간격 맞춤.
        int childCount = transform.childCount;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Vector3 eulerAngle = child.localRotation.eulerAngles;
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
            child.position = transform.position + direction * AtRange;
            child.localRotation = Quaternion.Euler(eulerAngle.x, angleStep * i, eulerAngle.z); // 프리팹 rotation을 타켓쪽으로 맞춤.
        }
    }
}
