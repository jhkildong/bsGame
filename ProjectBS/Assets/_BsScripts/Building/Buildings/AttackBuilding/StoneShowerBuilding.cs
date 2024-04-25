using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class StoneShowerBuilding : AttackBuilding_Area
{
    public GameObject myEffect;

    protected override void Start()
    {
        base.Start();
        AtkEvent.AddListener(SetActiveEffects);
    }

    void SetActiveEffects()
    {
        if (target != null && atkDelaying)
        {

            EffectPoolManager.Instance.SetActiveRangeObject<PointAtkEffectHit>(myEffect, effectPool, target, _finalDmg, _atkRadius, 0, 0, _atkSpeed,_hitDelay, _atkDuration);

        }
    }
}
