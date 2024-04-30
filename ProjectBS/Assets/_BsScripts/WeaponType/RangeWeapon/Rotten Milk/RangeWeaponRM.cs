using UnityEngine;

public class RangeWeaponRM : Bless
{
    short Count = 0;

    // Start is called before the first frame update
    void Start()
    {
        Count = 0;
        SetFowardPlayerLook();
    }

    // Update is called once per frame
    void Update()
    {
        if (CurLv >= 1)
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
        bullet.Ak = myStatus[Key.Attack];
        bullet.DelayTime = myStatus[Key.DelayTime];
        bullet.AtRange = myStatus[Key.AtRange];
    }
}
