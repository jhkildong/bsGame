using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster_Normal_", menuName = "Monster/NormalMonster", order = 0)]
public class NormalMonsterData : MonsterData
{
    /// <summary>
    /// normalMonsterData를 기반으로 몬스터 오브젝트 생성
    /// </summary>
    /// <returns>monster오브젝트에 부착한 NormalMonster 컴포넌트</returns>
    public override Monster CreateMonster()
    {
        GameObject monster = new GameObject(Name);
        monster.AddComponent<NormalMonster>().Init(this);

        return monster.GetComponent<NormalMonster>();
    }
}
