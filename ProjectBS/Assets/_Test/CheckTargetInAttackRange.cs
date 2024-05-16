using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class CheckTargetInAttackRange : ActionNode
{
    protected override void OnStart() {
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        if (context.monster.AttackTargetInRange())
        {
            context.monster.SetDirection(context.transform.forward);
            return State.Success;
        }
            
        else
            return State.Failure;
    }
}
