using UnityEngine;
using TheKiwiCoder;

public abstract class NormalMonster : Monster
{
    public NormalMonsterData NormalData => _data as NormalMonsterData;

    public override void Init(MonsterData data)
    {
        base.Init(data as NormalMonsterData);

        BehaviourTree tree = Resources.Load<BehaviourTree>(FilePath.MonsterBehaviourTree);
        GetComponent<BehaviourTreeRunner>().Init(tree);
    }




}
