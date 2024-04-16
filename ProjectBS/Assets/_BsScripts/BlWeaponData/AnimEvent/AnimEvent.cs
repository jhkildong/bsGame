using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    public event UnityAction AttackAct;
    public event UnityAction DeadAct;
    public event UnityAction<int> SkillAct;

    public void OnAttack()
    {
        AttackAct?.Invoke();
    }

    public void OnDead()
    {
        DeadAct?.Invoke();
    }

    public void OnSkill(int v)
    {
        SkillAct?.Invoke(v);
    }
}
