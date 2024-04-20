using UnityEngine;

public class Archer : PlayerComponent
{
    public override void OnAttackPoint()
    {
        //공격 이펙트 생성
        ArrowEffect arrow = ObjectPoolManager.Instance.GetEffect(MyEffect, Attack) as ArrowEffect;
        arrow.This.transform.SetPositionAndRotation(MyEffectSpawn.position, MyEffectSpawn.rotation);
        arrow.Shoot();
    }
}