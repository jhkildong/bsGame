using UnityEngine;

public class Mage : PlayerComponent
{
    private Transform handEffectPoint;

    private void Start()
    {
        
    }

    public override void OnAttackPoint()
    {
        MagicEffect magic = ObjectPoolManager.Instance.GetEffect(MyEffect, Attack) as MagicEffect;
        magic.This.transform.SetPositionAndRotation(MyEffectSpawn.position, MyEffectSpawn.rotation);
        magic.Shoot();
    }
}
