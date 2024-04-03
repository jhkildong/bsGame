using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBuilding : AttackBuildingBase
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
            EffectPoolManager.Instance.SetActiveObject<BuildingEffectHit>(myEffect, effectPool, target, _attackPower, _attackRadius);

        }
    }




}
