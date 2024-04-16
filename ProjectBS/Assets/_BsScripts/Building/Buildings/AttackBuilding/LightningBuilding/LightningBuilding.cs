using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBuilding : AttackBuilding_Area // 0412 º¯°æÀü -> AttackBuildingBase
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
            
            EffectPoolManager.Instance.SetActiveRangeObject<PointAtkEffectHit>(myEffect, effectPool, target, _finalDmg, _atkRadius, 0, 0,_hitDelay,_atkDuration);
            
        }
    }




}
