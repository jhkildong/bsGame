using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckAttackDuring : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        context.monster.SetDirection(Vector3.zero);
        if (!context.monster.AttackReady())
            return State.Running;
        else
            return State.Success;
    }
}
