using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckAttackDelay : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if(context.monster.AttackReady())
            return State.Success;
        else
            return State.Failure;
    }
}
