using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Yeon2;

public class BlessManager : Singleton<BlessManager>
{
    protected override void Awake()
    {
        base.Awake();
        Initialize();
    }

    //Resources/UI ������ �ִ� BlessData�� ������ ��ųʸ�
    public Dictionary<int, BlessData> BlessDict => _blessDict;
    private Dictionary<int, BlessData> _blessDict;

    private void Initialize()
    {
        //UIComponent�� ��ӹ��� ��� Ŭ������ Resources/UI �������� �ε��Ͽ� ��ųʸ��� ����
        BlessData[] BlessLists = Resources.LoadAll<BlessData>(FilePath.Bless);
        _blessDict = new Dictionary<int, BlessData>();
        foreach (BlessData data in BlessLists)
        {
            _blessDict.Add(data.ID, data);
        }
    }

    public Bless CreateBless(BlessID id)
    {
        if (_blessDict.ContainsKey((int)id))
        {
            Bless clone = _blessDict[(int)id].CreateClone();
            clone.transform.position += new Vector3(0, 0.7f, 0);
            return clone;
        }
        return null;
    }
}

public enum BlessID
{
    BOTS = 1010, RA = 1020, PF = 1030,
    LD = 1040, BOTCA = 1050, CS = 1060, VG = 1070,
    LP = 1080, RM = 1090, BOL = 1100,

}