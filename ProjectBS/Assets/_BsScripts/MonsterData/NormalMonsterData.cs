using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster_Normal_", menuName = "Monster/NormalMonster", order = 0)]
public class NormalMonsterData : MonsterData
{
    public override Monster CreateMonster()
    {
        GameObject monster = new GameObject(Name);
        monster.AddComponent<NormalMonster>().Init(this);
        monster.AddComponent<EnemyFollowPlayer>();
        Instantiate(MonsterPrefab, monster.transform);

        return monster.GetComponent<NormalMonster>();
    }
}
