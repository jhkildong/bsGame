using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Monster_Normal_", menuName = "Monster/NormalMonster", order = 0)]
public class NormalMonsterData : MonsterData
{
    /// <summary>
    /// normalMonsterData를 기반으로 몬스터 오브젝트 생성
    /// </summary>
    /// <returns> monster오브젝트에 부착한 NormalMonster 컴포넌트</returns>
    public override GameObject CreateClone()
    {
        GameObject clone = new GameObject(Name);
        clone.AddComponent<NormalMonster>().Init(this);

        return clone;
    }
}
