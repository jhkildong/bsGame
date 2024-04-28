using UnityEngine;
using Yeon2;

public class RangeWeaponRM : Bless
{
    public GameObject objectPrefab; // 생성할 프리펩

    short Level = 0;
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
        if (Level >= 1)
        {
            if (Count < 1)
            {
                Count++;
                SpawnWeapon();
            }
        }
    }

    public void OnOkSpawnRangeWaepon()
    {
        if (Level < 7)
        {
            Level++;
            LevelUp(Level);
            Debug.Log($"{Level}Level 입니다.");
        }
    }

    private void SpawnWeapon()
    {
        GameObject go = Instantiate(objectPrefab, transform); // 무기 생성
        var bullet = go.GetComponentInChildren<RangeWeaponRM_Bullet>();
        bullet.Ak = myStatus[Key.Attack];
        bullet.DelayTime = myStatus[Key.DelayTime];
        bullet.AtRange = myStatus[Key.AtRange];
    }
}
