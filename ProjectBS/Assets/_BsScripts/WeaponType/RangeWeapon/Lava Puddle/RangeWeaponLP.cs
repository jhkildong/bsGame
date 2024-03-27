using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponLP : MonoBehaviour
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
            case 3:  //  대미지 20% 증가.
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
            case 5:  //  지속시간이 50% 증가.
                if (Count < 1)
                {
                    Count++;
                    destroyTime += destroyTime * 0.5f;
                    Debug.Log("지속시간이 50% 증가");
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
            case 7:  //  4개가 된다.
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
        Vector3 randomPos = Random.insideUnitSphere * atRange;
        randomPos.y = 0.0f;
        GameObject bullet = Instantiate(objectPrefab, randomPos, Quaternion.identity);
        Destroy(bullet, destroyTime);
    }
}
