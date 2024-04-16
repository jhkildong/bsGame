using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class SpikeBuilding : AttackBuildingBase
{
    public GameObject myEffect;

    protected override void Start()
    {
        base.Start();
        //SetActiveEffects();
    }

    void SetActiveEffects()
    {
        if (target != null && atkDelaying)
        {
            //EffectPoolManager.Instance.SetActiveMeeleLastObject<BuildingMeeleHit>(myEffect, effectPool,effectPool.gameObject,_atkPower, _atkRadius,0,0,_atkDelay);
        }
    }
}
