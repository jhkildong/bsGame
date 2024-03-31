using UnityEngine;

[CreateAssetMenu(fileName = "NM_Single_", menuName = "Monster/NormalMonster/Single", order = 0)]
public class SingleMonsterData : NormalMonsterData
{
    protected override void OnEnable()
    {
        _type = MonsterType.Single;
    }

    public override Monster CreateClone()
    {
        GameObject go = new GameObject(Name);
        Monster clone = go.AddComponent<SingleMonster>();
        clone.Init(this);

        return clone;
    }
}
