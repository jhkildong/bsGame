using UnityEngine;

[CreateAssetMenu(fileName = "NM_Single_", menuName = "Monster/NormalMonster/Single", order = 0)]
public class SingleNormalMonsterData : NormalMonsterData
{
    public override GameObject CreateClone()
    {
        _type = MonsterType.Single;
        GameObject clone = new GameObject(Name);
        clone.AddComponent<SingleNormalMonster>().Init(this);

        return clone;
    }
}
