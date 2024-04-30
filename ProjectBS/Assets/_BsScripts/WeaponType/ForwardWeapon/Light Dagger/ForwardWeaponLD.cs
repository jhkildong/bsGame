using System.Collections;
using UnityEngine;

public class ForwardWeaponLD : Bless
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
                Debug.Log($"�ð� {myStatus[Key.ReTime]}");
            }
        }

    }


    private void SetSpawnWeapon()
    {
        var bullet = SpawnWeapon() as ForwardMovingWeapon; // ���� ����
        bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
        bullet.transform.localScale = new Vector3(myStatus[Key.Size], myStatus[Key.Size], myStatus[Key.Size]); //������
        bullet.Ak = myStatus[Key.Attack];
        bullet.Shoot(30);
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