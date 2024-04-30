using UnityEngine;

public class Mage : PlayerComponent
{
    [SerializeField]private Transform handEffectPoint;

    public override Effect MyEffect
    {
        get => base.MyEffect;
        set
        {
            base.MyEffect = value;

        }
    }

    public override void OnAttackPoint()
    {
        MagicEffect magic = ObjectPoolManager.Instance.GetEffect(MyEffect, Attack) as MagicEffect;
        magic.This.transform.SetPositionAndRotation(MyEffectSpawn.position, MyEffectSpawn.rotation);
        magic.Shoot();
    }
}
