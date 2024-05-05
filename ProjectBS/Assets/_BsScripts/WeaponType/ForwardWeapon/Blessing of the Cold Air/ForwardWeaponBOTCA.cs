using System.Collections;
using UnityEngine;

public class ForwardWeaponBOTCA : Bless
{
    float time = 0.0f;
    float WaitTime = 0.05f;
    Vector3 myDir;

    // Start is called before the first frame update
    void Start()
    {
        SetFowardPlayerLook();
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (CurLv >= 0)
        {
            if (time >= myStatus[Key.ReTime])
            {
                time = 0.0f;
                StartCoroutine(SpawnMultipleWeapons(myStatus[Key.Amount]));
                Debug.Log($"시간 {myStatus[Key.ReTime]}");
            }
        }
    }

    private void SetSpawnWeapon()
    {
        var bullet = SpawnWeapon() as ForwardMovingWeapon;
        myDir = Quaternion.Euler(0, Random.Range(0.0f, 360.0f), 0) * Vector3.forward;
        bullet.transform.SetPositionAndRotation(transform.position, Quaternion.LookRotation(myDir));
        bullet.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]); //사이즈
        bullet.Ak = myStatus[Key.Attack];
        bullet.Shoot(25);
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