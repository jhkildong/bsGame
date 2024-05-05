using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponRM : Bless
{
    short Count = 0;
    private List<Weapon> myChildren;

    // Start is called before the first frame update
    void Start()
    {
        Count = 0;
        myChildren = new List<Weapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurLv >= 0)
        {
            if (Count < 1)
            {
                Count++;
                SetSpawnWeapon();
            }
        }
    }

    private void SetSpawnWeapon()
    {
        var bullet = SpawnWeapon() as RangeWeaponRM_Bullet;
        bullet.transform.SetParent(transform);
        bullet.transform.SetPositionAndRotation(transform.position, transform.rotation);
        bullet.Ak = myStatus[Key.Attack];
        bullet.DelayTime = myStatus[Key.DelayTime];
        bullet.AtRange = myStatus[Key.AtRange];
    }
}
