using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bless : MonoBehaviour
{
    public BlessData Data => _data;
    public int CurLv => _curLevel;
    public Dictionary<string, float> MyStatus => myStatus;
        
    [SerializeField] protected BlessData _data;
    protected int _curLevel;
    protected static int _maxLevel = 7;

    [SerializeField] protected Weapon weaponPrefab;
    protected Transform rotatingBody;
    

    //레벨업시 변경될 스테이터스를 저장하는 딕셔너리
    protected Dictionary<string, float> myStatus = new Dictionary<string, float>();

    public virtual void Init(BlessData data)
    {
        _data = data;
        rotatingBody = GameManager.Instance.Player.RotatingBody;
        _curLevel = 0;
        weaponPrefab.SetID(data.ID);
        ObjectPoolManager.Instance.SetPool(weaponPrefab, 10, 10);

        foreach (var lvData in data.LvDataList)
        {
            myStatus.Add(lvData.name, lvData.defaultValue);
        }
    }

    public virtual void LevelUp()
    {
        _curLevel++;
        foreach (var lvData in _data.LvDataList)
        {
            myStatus[lvData.name] = lvData[_curLevel];
        }
        if (_curLevel >= _maxLevel)
        {
            BlessManager.Instance.RemoveBlessInSelectPool(Data.ID);
            return;
        }
    }

    protected void SetFowardPlayerLook()
    {
        transform.SetParent(GameManager.Instance.Player.RotatingBody);
        transform.localRotation = Quaternion.identity;
    }

    protected Weapon SpawnWeapon()
    {
        return ObjectPoolManager.Instance.GetObj(weaponPrefab) as Weapon;
    }
}

