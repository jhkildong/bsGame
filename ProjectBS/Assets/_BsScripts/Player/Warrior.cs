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
        GameObject go = ObjectPoolManager.Instance.GetEffect(MyEffect, Attack).This.gameObject;
        go.transform.SetPositionAndRotation(MyEffectSpawn.position, Quaternion.Euler(0.0f, MyEffectSpawn.rotation.eulerAngles.y, _attackDir));
    }
}