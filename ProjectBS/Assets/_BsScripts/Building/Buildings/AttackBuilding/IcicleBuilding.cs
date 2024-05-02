using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IcicleBuilding : AttackBuilding_Projectile
{
    //public GameObject myProjectile;
    public GameObject muzzle;


    protected override void Start()
    {
        base.Start();
        AtkEvent.AddListener(SetActiveEffects);
    }

    void SetActiveEffects()
    {
        if (target != null && atkDelaying)
        {
            relativeDir = (target.transform.position - muzzle.transform.position).normalized;
            relativeDir.y = 0;
            EffectPoolManager.Instance.SetActiveProjectileObject(atkEffect, effectPool, muzzle, _atkId, _atkPower,
                _atkProjectileSize, _atkProjectileSpeed, _atkProjectileRange, _atkCanPen, _atkPenCount,relativeDir);

        }
    }
}
