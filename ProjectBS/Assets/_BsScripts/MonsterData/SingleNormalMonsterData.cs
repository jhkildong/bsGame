using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NM_Single_", menuName = "Monster/NormalMonster/Single", order = 0)]
public class SingleNormalMonsterData : NormalMonsterData
{
    /// <summary>
    /// SingleNormalMonsterData�� ������� ���� ������Ʈ ����
    /// </summary>
    /// <returns> monster������Ʈ�� ������ SingleNormalMonsterData ������Ʈ</returns>
    public override GameObject CreateClone()
    {
        GameObject clone = new GameObject(Name);
        clone.AddComponent<SingleNormalMonster>().Init(this);

        return clone;
    }
}
