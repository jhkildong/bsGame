using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobBless : Bless
{
    public override void Init(BlessData data)
    {
        _data = data;
        _curLevel = 0;

        foreach (var lvData in data.LvDataList)
        {
            myStatus.Add(lvData.name, lvData.defaultValue);
        }
    }
}
