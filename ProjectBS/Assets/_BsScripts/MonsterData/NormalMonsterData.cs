using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster_Normal_", menuName = "Monster/NormalMonster", order = 0)]
public class NormalMonsterData : MonsterData
{
    /// <summary>
    /// normalMonsterData�� ������� ���� ������Ʈ ����
    /// </summary>
    /// <returns>monster������Ʈ�� ������ NormalMonster ������Ʈ</returns>
    public override Monster CreateMonster()
    {
        GameObject monster = new GameObject(Name);
        monster.AddComponent<NormalMonster>().Init(this);

        return monster.GetComponent<NormalMonster>();
    }
}
