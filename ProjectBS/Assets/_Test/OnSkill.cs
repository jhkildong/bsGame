using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class OnSkill : ActionNode
{
    public int skillIndex;
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        BossMonster boss = context.monster as BossMonster;
        boss.SkillList[skillIndex].OnSkill();
        return State.Success;
    }
}
