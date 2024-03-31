using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ForwardWeaponBOTCA : MonoBehaviour
{
    public Transform clonesParent; // 생성한 프리펩들 보관할 곳
    public Transform myTarget; // 따라갈 타겟
    public Transform myRotate; // 따라서 회전할 타겟
    public GameObject weaponPrefab; // 생성할 프리팹

    public float reTime = 2.0f; // 공격속도
    public float waitTime = 0.05f; // 재 생성 간격

    float time = 0.0f; // 시간 저장 변수

    public short Level = 0;
    short weaponCount = 0; // 무기 개수
    short Count = 0;
    // Start is called before the first frame update
    void Start()
    {
        Level = weaponCount = Count =  0;
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
                if (time >= reTime)
                {
                    time = 0.0f;
                    SpawnWeapon();
                }
                break;
            case 2:  //  2개가 된다.
                if (Count < 2)
                {
                    Count++;
                    weaponCount++;
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 3:  //  대미지 20% 증가
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 20% 증가");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 4:  //  3개가 된다.
                if (Count < 1)
                {
                    Count++;
                    weaponCount++;
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 5:  //  공격속도 50% 증가
                if (Count < 1)
                {
                    Count++;
                    reTime -= reTime * 0.5f;
                    Debug.Log("공격속도 50% 증가");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 6:  //  4개가 된다.
                if (Count < 1)
                {
                    Count++;
                    weaponCount++;
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 7:  //  5개가 된다.
                if (Count < 1)
                {
                    Count++;
                    weaponCount++;
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;

        }
    }
     IEnumerator SpawnMultipleWeapons(int v)
    {
        for (int i = 0; i < v; i++)
        {
            SpawnWeapon();
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void SpawnWeapon()
    {
        GameObject bulletBOTCA = Instantiate(weaponPrefab, transform.position, transform.rotation);
        bulletBOTCA.transform.SetParent(clonesParent);
        Rigidbody SwordRigidbody = bulletBOTCA.GetComponent<Rigidbody>();
    }
}