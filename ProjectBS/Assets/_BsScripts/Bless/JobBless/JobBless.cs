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
        UpdateStatusToPlayer();
    }
    public override void LevelUp()
    {
        _curLevel++;

        int tempLevel = _curLevel;
        if (Data.ID ==(int)BlessID.MAGE && _curLevel >= _maxLevel)
        {
            GameManager.Instance.Player.MageLv7SkillReady = true;
            tempLevel--;
        }
        foreach (var lvData in _data.LvDataList)
        {
            myStatus[lvData.name] = lvData[tempLevel];
        }
        if (_curLevel >= _maxLevel)
        {
            BlessManager.Instance.RemoveBlessInSelectPool(Data.ID);
        }
        UpdateStatusToPlayer();
    }
    
    public void MageLv7SkillOn()
    {
        if(Data.ID == (int)BlessID.MAGE)
        {
            foreach (var lvData in _data.LvDataList)
            {
                myStatus[lvData.name] = lvData[7];
            }
            UpdateStatusToPlayer();
        }
    }

    public void MageLv7SkillOff()
    {
        if (Data.ID == (int)BlessID.MAGE)
        {
            foreach (var lvData in _data.LvDataList)
            {
                myStatus[lvData.name] = lvData[6];
            }
            UpdateStatusToPlayer();
        }
    }

    //TDOO: 각 직업마다 스크립트 분리 필요
    private void UpdateStatusToPlayer()
    {
        switch (Data.ID)
        {
            case (int)BlessID.WARRIOR:
                GameManager.Instance.Player.UpdateStatus(
                    myStatus[Key.Attack], myStatus[Key.SkillCoolTime], castingTime: myStatus[Key.SkillCastTime]);
                break;
            case (int)BlessID.ARCHER:
                GameManager.Instance.Player.UpdateStatus(
                    myStatus[Key.Attack], myStatus[Key.SkillCoolTime], atksp: myStatus[Key.AttackSp]);
                GameManager.Instance.Player.MaxSkillStack = (int)myStatus[Key.SkillAmount];
                break;
            case (int)BlessID.MAGE:
                GameManager.Instance.Player.UpdateStatus(
                    myStatus[Key.Attack], myStatus[Key.SkillCoolTime], myStatus[Key.AttackSp], myStatus[Key.SkillCastTime]);
                break;
        }
    }
}
