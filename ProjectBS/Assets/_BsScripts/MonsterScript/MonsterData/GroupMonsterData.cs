using UnityEngine;

[CreateAssetMenu(fileName = "NM_Group_", menuName = "Monster/NormalMonster/Group", order = 1)]
public class GroupMonsterData : NormalMonsterData
{
    public int Amount => _amount;

    [SerializeField] private int _amount;

    protected override void OnEnable()
    {
        _type = MonsterType.Group;
    }

    public override Monster CreateClone()
    {
        GameObject go = new GameObject(Name);
        Monster clone = go.AddComponent<GroupMonster>();
        clone.Init(this);

        return clone;
    }
}