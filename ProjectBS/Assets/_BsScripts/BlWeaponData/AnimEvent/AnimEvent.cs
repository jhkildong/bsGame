using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AnimEvent : MonoBehaviour
{
    public UnityEvent attackAct;
    public UnityEvent deadAct;
    public UnityEvent<int> skillAct;

    public void OnAttack()
    {
        attackAct?.Invoke();
    }

    public void OnDead()
    {
        deadAct?.Invoke();
    }

    public void OnSkill(int v)
    {
        skillAct?.Invoke(v);
    }
}
