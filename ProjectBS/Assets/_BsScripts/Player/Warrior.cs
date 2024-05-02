using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerComponent
{
    public override Job MyJob => Job.Warrior;

    private void Start()
    {
        MyAnim.GetBehaviour<ResetDir>().ResetDirAct += ResetAtackDir;
    }
    
    private bool attackDirSwitch = false;
    private void ChangeAttackDir()
    {
        attackDirSwitch = !attackDirSwitch;
    }
    public void ResetAtackDir()
    {
        attackDirSwitch = false;
    }
   
    public override void OnAttackPoint()
    {
        float _attackDir = attackDirSwitch ? 0f : 180f;
        ChangeAttackDir();

        //공격 이펙트 생성
        GameObject go = ObjectPoolManager.Instance.GetEffect(MyEffect, Attack, MyJobBless.MyStatus[Key.Size]).This.gameObject;
        go.transform.SetPositionAndRotation(MyEffectSpawn.position, Quaternion.Euler(0.0f, MyEffectSpawn.rotation.eulerAngles.y, _attackDir));
    }

    public override void SetSkillAct(Player player)
    {
        player.OnSkillAct += OnSkill;
        player.OffSkillAct += OffSkill;
    }

    private void OnSkill()
    {
        MySkillEffect.Attack = MyJobBless.MyStatus[Key.SkillAttack];
        MySkillEffect.Size = MyJobBless.MyStatus[Key.SkillSize];
        OnSkillEffect(1);
        SetRigWeight(0);
        GameManager.Instance.Player.RotatingBody.GetComponent<LookAtPoint>().enabled = false;   //스킬 사용시 마우스 방향으로 회전하는 스크립트 비활성화
    }

    private void OffSkill()
    {
        OnSkillEffect(0);
        SetRigWeight(1);
        GameManager.Instance.Player.RotatingBody.GetComponent<LookAtPoint>().enabled = true;    //스킬 사용 종료시 마우스 방향으로 회전하는 스크립트 활성화
    }
}