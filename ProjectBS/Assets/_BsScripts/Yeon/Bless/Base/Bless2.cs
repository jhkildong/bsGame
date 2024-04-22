using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bless2 : MonoBehaviour
{
    public Bless2Data Data => _data;
    [SerializeField] private Bless2Data _data;

    protected Dictionary<string, float> myStatus = new Dictionary<string, float>();

    public void Init(Bless2Data data)
    {
        _data = data;

        foreach(var lvData in data.LvDataList)
        {
            myStatus.Add(lvData.name, lvData.defaultValue);
        }
    }

    public void LevelUp(int level)
    {
        if (level < 0 || level >= 7)
            return;

        foreach (var lvData in _data.LvDataList)
        {
            myStatus[lvData.name] = lvData[level];
        }
    }
}
