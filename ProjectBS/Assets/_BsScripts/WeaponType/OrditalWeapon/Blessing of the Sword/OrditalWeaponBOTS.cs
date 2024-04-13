using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OrditalWeaponBOTS : MonoBehaviour
{
    public Transform myTarget; // 따라갈 타겟
    public GameObject weaponPrefab; // 생성한 프리펩
    public float rotSpeed = 30.0f; // 공전 속도
    public float attakRange = 1.0f; // 범위

    public short Level = 0;
    short weaponCount = 0;
    short Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Level = weaponCount = Count = 0;
        if (myTarget != null) transform.SetParent(myTarget);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, -rotSpeed * Time.deltaTime); // 공전
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
                if (weaponCount < 1)
                {
                    weaponCount++;
                    SpawnWeapon();
                }
                break;
            case 2:  //  2개가 된다.
                if (weaponCount < 2)
                {
                    weaponCount++;
                    SpawnWeapon();
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
                if (weaponCount < 3)
                {
                    weaponCount++;
                    SpawnWeapon();
                }
                break;
            case 5:  //  회전속도 50% 증가.
                if (Count < 1)
                {
                    Count++;
                    rotSpeed += rotSpeed * 0.5f;
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
                if (weaponCount < 4)
                {
                    weaponCount++;
                    SpawnWeapon();
                }
                break;

        }
    }


    private void SpawnWeapon()
    {
        GameObject bulletBOTS = Instantiate(weaponPrefab, transform); // 무기 생성

        // 생성한 무기 간격을 일정하게 맞춤.
        int childCount = transform.childCount;
        float angleStep = 360.0f / childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform child = transform.GetChild(i);
            Vector3 eulerAngle = child.localRotation.eulerAngles;
            Vector3 direction = Quaternion.Euler(0, angleStep * i, 0) * transform.forward;
            child.position = transform.position + direction * attakRange;
            child.localRotation = Quaternion.Euler(eulerAngle.x, angleStep * i, eulerAngle.z);
        }
    }
}
