using System.Collections;
using UnityEngine;

public class RangeWeaponBOL : Bless
{    
    float time = 0.0f;
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

        if (CurLv >= 1)
        {
            if (time >= myStatus[Key.ReTime])
            {
                time = 0.0f;
                StartCoroutine(SpawnMultipleWeapons(myStatus[Key.Amount]));
            }
        }
    }

    private void SetSpawnWeapon()
    {
        Vector3 randomPos = Random.insideUnitSphere * myStatus[Key.AtRange];
        Vector3 spawnPos = transform.position + randomPos;
        spawnPos.y = 0.0f;
        var bullet = SpawnWeapon(); // 公扁 积己
        bullet.transform.position = spawnPos; // 公扁 困摹
        bullet.Ak = myStatus[Key.Attack];
    }

    IEnumerator SpawnMultipleWeapons(float v)
    {
        for (int i = 0; i < v; i++)
        {
            SetSpawnWeapon();
            yield return new WaitForSeconds(WaitTime);
        }
    }
}


