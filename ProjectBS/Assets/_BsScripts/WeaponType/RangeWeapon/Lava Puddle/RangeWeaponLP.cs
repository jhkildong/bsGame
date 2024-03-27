using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponLP : MonoBehaviour
{
    public Transform myTarget;
    public GameObject objectPrefab;

    public float atRange = 10.0f; // �����ݰ�
    public float reTime = 2.0f; // ���ݼӵ�
    public float waitTime = 0.2f; // �� ���� ����
    public float destroyTime = 3.0f; // �������� �ð�

    float time = 0.0f;

    public short Level = 0;
    short weaponCount = 0; // ���� ����
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
            Debug.Log($"{Level}Level �Դϴ�.");
        }
    }

    void SwitchUpdate()
    {
        switch (Level)
        {
            case 1:  //  1�� ����
                if (time >= reTime)
                {
                    time = 0.0f;
                    SpawnWeapon();
                }
                break;
            case 2:  //  2���� �ȴ�.
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
            case 3:  //  ����� 20% ����.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 30% ����");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 4:  //  3���� �ȴ�.
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
            case 5:  //  ���ӽð��� 50% ����.
                if (Count < 1)
                {
                    Count++;
                    destroyTime += destroyTime * 0.5f;
                    Debug.Log("���ӽð��� 50% ����");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 6:  //  ����� 30% ����.
                if (Count < 1)
                {
                    Count++;
                    Debug.Log("����� 30% ����");
                }
                if (time >= reTime)
                {
                    time = 0.0f;
                    StartCoroutine(SpawnMultipleWeapons(weaponCount));
                }
                break;
            case 7:  //  4���� �ȴ�.
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
