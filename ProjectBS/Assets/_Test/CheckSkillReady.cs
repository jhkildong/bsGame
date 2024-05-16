using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckSkillReady : ActionNode
{
    public int skillIndex;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        BossMonster boss = context.monster as BossMonster;
        if (boss.SkillList[skillIndex].isReady)
            return State.Success;
        else
            return State.Failure;
    }
}
