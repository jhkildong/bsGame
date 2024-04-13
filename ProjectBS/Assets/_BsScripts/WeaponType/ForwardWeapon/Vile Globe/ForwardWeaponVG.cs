using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using UnityEngine;

public class ForwardWeaponVG : MonoBehaviour
{
    public Transform myTarget; // 따라갈 타겟
    public Transform myRotate; // 따라서 회전할 타겟
    public GameObject weaponPrefab; // 생성할 프리팹

    public float maxRange = 1.0f; // 최대 폭
    public float minRange = -1.0f; // 최소 폭

    public float prefabScale = 1.0f; // 프리펩 크기
    public float destroyTime = 0.5f; // 생성한 무기 없는 시간
    public float atRange = 5.0f; // 플레이어로부터 무기 생성거리

    public float reTime = 2.0f; // 공격속도
    public float waitTime = 0.05f; // 재 생성 간격

    float time = 0.0f; // 시간 저장 변수

    public short Level = 0;
    short weaponCount = 0; // 무기 개수
    short Count = 0; // 단순 반복 횟수

    // Start is called before the first frame update
    void Start()
    {
        Level = weaponCount = Count = 0;
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

        GameObject bulletVG = Instantiate(weaponPrefab, transform); // 무기 생성
        int childCount = transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            Transform child = transform.GetChild(i);
            Vector3 direction = Quaternion.Euler(0.0f, 0.0f, 0.0f) * transform.forward;
            float randomXPos = Random.Range(minRange, maxRange); // 랜덤 최소, 최대 폭 설정
            child.position = transform.position + new Vector3(randomXPos, 0.0f, 0.0f) + direction * atRange;

            child.localScale = new Vector3(prefabScale, prefabScale, prefabScale);
        }
        bulletVG.transform.SetParent(null);
        Destroy(bulletVG, destroyTime);

    }
}