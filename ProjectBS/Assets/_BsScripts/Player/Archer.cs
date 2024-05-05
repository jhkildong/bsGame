using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayerComponent
{
    public override Job MyJob => Job.Archer;
    [SerializeField]private GameObject SkillRangeMaker;
    public Stack<PlayerSkill> skillEffectStack;

    private void Start()
    {
        MySkillEffect.transform.SetParent(null);
        skillEffectStack = new Stack<PlayerSkill>();
        skillEffectStack.Push(MySkillEffect);

        for(int i = 0; i < 4; i++)
        {
            PlayerSkill clone = Instantiate(MySkillEffect);
            clone.gameObject.SetActive(false);
            skillEffectStack.Push(clone);
        }

        MyAnimEvent.SkillAct += OnSkillEffect;
        MyAnimEvent.SkillAct += OnArcherSkill;
    }

    private void OnArcherSkill(int i)
    {
        GameManager.Instance.Player.RotatingBody.GetComponent<LookAtPoint>().enabled = true;
    }

    public override void OnSkillEffect(int onSkill)
    {
        if (onSkill == 0)
            MySkillEffect.gameObject.SetActive(false);
        else if (onSkill == 1)
        {
            MySkillEffect = skillEffectStack.Pop();
            (MySkillEffect as ArcherSkill).myParent = this;
            MySkillEffect.Attack = MyJobBless.MyStatus[Key.SkillAttack];
            MySkillEffect.Size = MyJobBless.MyStatus[Key.SkillSize];
            MySkillEffect.gameObject.SetActive(true);
            MySkillEffect.transform.position = SkillRangeMaker.transform.position;
        }
            
    }

    public override void OnAttackPoint()
    {
        //공격 이펙트 생성
        ArrowEffect arrow = ObjectPoolManager.Instance.GetEffect(MyEffect, Attack) as ArrowEffect;
        arrow.This.transform.SetPositionAndRotation(MyEffectSpawn.position, MyEffectSpawn.rotation);
        arrow.Shoot();
    }

    public override void SetSkillAct(Player player)
    {
        player.OnSkillAct += OnSkill;
        player.OffSkillAct += OffSkill;
    }

    private void OnSkill()
    {
        SkillRangeMaker.SetActive(true);
        SkillRangeMaker.transform.localScale = Vector3.one * MyJobBless.MyStatus[Key.SkillSize];
    }

    private void OffSkill()
    {
        GameManager.Instance.Player.isCastingSkill = true;          //스킬 사용 상태 true, 애니메이션이 끝날 때 false로 호출(ArcherSkillState.cs)
        GameManager.Instance.Player.SetInputState(false);
        SkillRangeMaker.SetActive(false);
    }
}