using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Monster_Normal_", menuName = "Monster/NormalMonster", order = 0)]
public class NormalMonsterData : MonsterData
{
    /// <summary>
    /// normalMonsterData�� ������� ���� ������Ʈ ����
    /// </summary>
    /// <returns> monster������Ʈ�� ������ NormalMonster ������Ʈ</returns>
    public override GameObject CreateClone()
    {
        GameObject clone = new GameObject(Name);
        clone.AddComponent<NormalMonster>().Init(this);

        return clone;
    }
}
