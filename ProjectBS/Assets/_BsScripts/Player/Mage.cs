using UnityEngine;

public class Mage : PlayerComponent
{
    public override Job MyJob => Job.Mage;

    [SerializeField]private Transform handEffectPoint;

    public void SetHandEffect(GameObject handEffect)
    {
        if(handEffectPoint == null)
        {
            handEffectPoint = transform.Find("HandEffect");
        }
        handEffect.transform.SetParent(handEffectPoint);
        handEffect.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
    }

    public override void OnAttackPoint()
    {
        MagicEffect magic = ObjectPoolManager.Instance.GetEffect(MyEffect, Attack) as MagicEffect;
        magic.This.transform.SetPositionAndRotation(MyEffectSpawn.position, MyEffectSpawn.rotation);
        magic.Shoot();
    }
}
