using UnityEngine;

public class Archer : PlayerComponent
{
    public override Job MyJob => Job.Archer;
    [SerializeField]private GameObject SkillRangeMaker;
    private GameObject clone;

    public void ShowRange()
    {
        if(clone == null)
        {
            clone = Instantiate(SkillRangeMaker);
        }
        clone.SetActive(true);
    }

    public void HideRange()
    {
        clone.SetActive(false);
    }

    public override void OnAttackPoint()
    {
        //공격 이펙트 생성
        ArrowEffect arrow = ObjectPoolManager.Instance.GetEffect(MyEffect, Attack) as ArrowEffect;
        arrow.This.transform.SetPositionAndRotation(MyEffectSpawn.position, MyEffectSpawn.rotation);
        arrow.Shoot();
    }
}