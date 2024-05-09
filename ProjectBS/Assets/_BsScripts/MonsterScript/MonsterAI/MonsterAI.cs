using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAI : MonoBehaviour
{
    private BehaviorTreeRunner _BTRunner;
    /*INode SettingBT()
    {
        return new SelectorNode (
            new List<INode>()
            {
                new SequenceNode (
                     new List<INode>()
                     {
                         new ActionNode(CheckAttacking),
                         new ActionNode(CheckEnemyWithinAttackRange),
                         new ActionNode(Attack)
                     }),
                new SequenceNode (
                    new List<INode>()
                    {
                        new ActionNode(ChechkDetectEnemy),
                        new ActionNode(MoveToDectetEnemy),
                    }),
                new ActionNode(MoveToOriginPosition)
            }
        );
    }*/

    private void Awake()
    {
        //_BTRunner = new BehaviorTreeRunner(SettingBT());
    }

    private void Update()
    {
        _BTRunner.Operate();
    }


}
