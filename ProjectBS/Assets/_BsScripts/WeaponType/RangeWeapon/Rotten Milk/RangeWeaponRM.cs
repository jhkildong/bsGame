using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponRM : MonoBehaviour
{
    public Transform myTarget;
    public GameObject objectPrefab;

    public float atRange = 10.0f; // 생성반경
    public float reTime = 2.0f; // 공격속도
    public float waitTime = 0.2f; // 재 생성 간격
    public float destroyTime = 3.0f; // 없어지는 시간

    float time = 0.0f;

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
                if (time >= reTime)
                {
                    time = 0.0f;
                    SpawnWeapon();
                }
                break;
            case 2:  //  공격 범위가 20% 증가
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("공격 범위가 20% 증가");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    SpawnWeapon();
                }
                break;
            case 3:  //  대미지 20% 증가.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 20% 증가");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    SpawnWeapon();
                }
                break;
            case 4:  //  2개가 된다.
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
            case 5:  //  공격 딜레이가 30% 감소.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("공격 딜레이가 30% 감소.");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 6:  //  대미지 30% 증가.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("대미지 30% 증가");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 7:  //  공격 속도 40% 증가
                if (Count < 1)
                {
                    Count++;
                    reTime -= reTime *= 0.4f;
                    Debug.Log("공격 속도 40% 증가");
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
        Vector3 randomPos = Random.insideUnitSphere * atRange;
        randomPos.y = 0.0f;
        GameObject bullet = Instantiate(objectPrefab, randomPos, Quaternion.identity);
        Destroy(bullet, destroyTime);
    }
}
