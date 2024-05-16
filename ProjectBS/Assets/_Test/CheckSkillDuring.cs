using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckSkillDuring : ActionNode
{
    public int skillIndex;
    public bool stopMoving;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        BossMonster boss = context.monster as BossMonster;
        if(stopMoving) boss.SetDirection(Vector3.zero);
        else boss.SetDirection(context.transform.forward);

        if (boss.SkillList[skillIndex].isDuring)
            return State.Running;
        else
            return State.Success;
    }
}
