using UnityEngine;

public class Archer : PlayerComponent
{
    public override void OnAttackPoint()
    {
        //���� ����Ʈ ����
        ArrowEffect arrow = ObjectPoolManager.Instance.GetEffect(MyEffect, Attack) as ArrowEffect;
        arrow.This.transform.SetPositionAndRotation(MyEffectSpawn.position, MyEffectSpawn.rotation);
        arrow.Shoot();
    }
}