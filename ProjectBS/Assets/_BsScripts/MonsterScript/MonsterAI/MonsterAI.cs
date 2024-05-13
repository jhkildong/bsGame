using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MonsterAI : MonoBehaviour
{
    LayerMask targetLayer = (int)(BSLayerMasks.Player | BSLayerMasks.Building);

    float attack= 1.0f;
    float radius = 1.0f;
    private Vector3 BottomPos
        => new Vector3(transform.position.x, transform.position.y + radius, transform.position.z);

    private BehaviorTreeRunner _BTRunner;

    INode SettingBT()
    {
        return new SelectorNode (
            new List<INode>()
            {
                new SequenceNode (
                     new List<INode>()
                     {
                         new ActionNode(CheckTargetInAttackRange),
                         new ActionNode(CheckAttackTime),
                         new ActionNode(Attack)
                     }),
                new ActionNode(MoveToPlayer)
            }
        );
    }

    float attackTime = 1.0f;
    float playTime = 0.0f;
    IDamage myTarget;


    protected virtual INode.NodeState CheckTargetInAttackRange()
    {
        if (myTarget == null)
            return INode.NodeState.Failure;
        else
            return INode.NodeState.Success;
    }

    protected virtual INode.NodeState CheckAttackTime()
    {
        if (playTime <= 0.0f)
        {
            playTime = attackTime;
            return INode.NodeState.Success;
        }
        else
        {
            playTime -= Time.deltaTime;
            return INode.NodeState.Running;
        }
    }

    protected virtual INode.NodeState Attack()
    {
        if (myTarget != null)
        {
            myTarget.TakeDamage(attack);
            return INode.NodeState.Success;
        }
        return INode.NodeState.Failure;
    }

    protected virtual INode.NodeState MoveToPlayer()
    {
        return INode.NodeState.Success;
    }


    private void Awake()
    {
        //_BTRunner = new BehaviorTreeRunner(SettingBT());
    }

    private void Update()
    {
        _BTRunner.Operate();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            if (Vector3.Dot(collision.transform.position - transform.position, transform.forward) > 0)
            {
                IDamage target = collision.gameObject.GetComponent<IDamage>();
                if (target != null)
                {
                    myTarget = target;
                }
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (((1 << collision.gameObject.layer) & targetLayer) != 0)
        {
            if (myTarget == collision.gameObject.GetComponent<IDamage>())
            {

                myTarget = null;
            }
        }
    }
}
