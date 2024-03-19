using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NM_Single_", menuName = "Monster/NormalMonster/Single", order = 0)]
public class SingleNormalMonsterData : NormalMonsterData
{
    /// <summary>
    /// SingleNormalMonsterData를 기반으로 몬스터 오브젝트 생성
    /// </summary>
    /// <returns> monster오브젝트에 부착한 SingleNormalMonsterData 컴포넌트</returns>
    public override GameObject CreateClone()
    {
        GameObject clone = new GameObject(Name);
        clone.AddComponent<SingleNormalMonster>().Init(this);

        return clone;
    }
}
