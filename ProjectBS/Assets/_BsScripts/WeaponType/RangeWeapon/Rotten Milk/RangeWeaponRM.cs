using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponRM : MonoBehaviour
{
    public Transform myTarget;
    public GameObject objectPrefab;

    public short Level = 0;
    short weaponCount = 0; // 무기 개수
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
                if (weaponCount < 1)
                {
                    weaponCount++;
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
        GameObject bulletRM = Instantiate(objectPrefab, transform);
        bulletRM.transform.SetParent(myTarget);
    }
}
