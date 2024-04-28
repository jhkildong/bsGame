using System.Collections;
using UnityEngine;
using Yeon2;

public class RangeWeaponLP : Bless
{
    public GameObject weaponPrefab;

    float time = 0.0f;
    short Level = 0;
    float WaitTime = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        SetFowardPlayerLook();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (Level >= 1)
        {
            if (time >= myStatus[Key.ReTime])
            {
                time = 0.0f;
                StartCoroutine(SpawnMultipleWeapons(myStatus[Key.Amount]));
                Debug.Log($"�ð� {myStatus[Key.ReTime]}");
            }
        }

    }

    public void OnOkSpawnRangeWaepon()
    {
        if (Level < 7)
        {
            Level++;
            LevelUp(Level);
            Debug.Log($"{Level}Level �Դϴ�.");
        }
    }

    private void SpawnWeapon()
    {
        Vector3 randomPos = Random.insideUnitSphere * myStatus[Key.AtRange];
        Vector3 spawnPos = transform.position + randomPos;
        spawnPos.y = 0.0f;
        GameObject go = Instantiate(weaponPrefab, spawnPos, Quaternion.identity); // ���� ����
        Destroy(go, myStatus[Key.DestroyTime]);

        var bullet = go.GetComponentInChildren<Weapon>();
        bullet.Ak = myStatus[Key.Attack];

    }

    IEnumerator SpawnMultipleWeapons(float v)
    {
        for (int i = 0; i < v; i++)
        {
            SpawnWeapon();
            yield return new WaitForSeconds(WaitTime);
        }
    }
}
