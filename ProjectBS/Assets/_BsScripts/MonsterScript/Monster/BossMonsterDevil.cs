using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterDevil : BossMonster
{
    protected override void InitCollider(float radius)
    {
        col = gameObject.AddComponent<CapsuleCollider>();
        (col as CapsuleCollider).radius = radius;
        (col as CapsuleCollider).height = 2.5f;
        (col as CapsuleCollider).center = new Vector3(0, 1.25f, 0);
    }

    public override void Init(MonsterData data)
    {
        base.Init(data);
        MonsterData servantImpData = Resources.Load<MonsterData>(FilePath.Imp);
        servantImp = servantImpData.CreateClone();
        ObjectPoolManager.Instance.ReleaseObj(servantImp);
        GameObject go = new GameObject("AIPerception");
        go.transform.SetParent(this.transform);
        go.AddComponent<AIPerception>().Init((int)BSLayerMasks.Player, FindTarget, LostTarget);
        Com.MyAnimEvent.AttackAct += OnAttackPoint;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if(Data == null)
            return;
        AddStateChange(AdditionalState.None);
        skill1Timer = 0.0f;
        Com.MyAnim.SetBool(AnimParam.isMoving, true);
        StartCoroutine(Skill1CoolDown());
        StartCoroutine(Skill2Cooldown());
    }

    private void FindTarget(Transform target)
    {
        attackTarget = target.GetComponent<IDamage>();
        ChangeState(State.Attack);
    }

    private void LostTarget()
    {
        
    }



    #region AdditionalState
    private enum AdditionalState
    {
        None,
        Skill1,
        Skill2
    }

    private float skill1Cooltime = 20.0f;
    private float skill2Cooltime = 120.0f;
    private float skill1Timer;
    private AdditionalState addState = AdditionalState.None;
    private List<IPoolable> impList = new List<IPoolable>();

    [SerializeField] private Monster servantImp;

    private IEnumerator Skill1CoolDown()
    {
        int attackCount = 5;
        var WaitForPlayTime = new WaitForSeconds(0.4f);
        while (myState == State.Attack)
        {
            while (Com.MyAnim.GetBool(AnimParam.OnSkill2))
            {
                yield return null;
            }
            skill1Timer -= Time.deltaTime;
            if (skill1Timer <= 0)
            {
                AddStateChange(AdditionalState.Skill1);
                Com.MyAnim.SetBool(AnimParam.OnSkill1, true);
                moveSpeed = Data.Sp * 2f;
                for(int i = 0; i < attackCount; i++)
                {
                    Collider[] cols = Physics.OverlapSphere(transform.position + new Vector3(0, 1.0f ,0), 2.0f, attackMask);
                    foreach (Collider col in cols)
                    {
                        col.GetComponent<IDamage>()?.TakeDamage(Attack);
                    }
                    yield return WaitForPlayTime;
                }
                Com.MyAnim.SetBool(AnimParam.OnSkill1, false);
                AddStateChange(AdditionalState.None);
                moveSpeed = Data.Sp;
                skill1Timer = skill1Cooltime;
            }
            yield return null;
        }
    }

    private IEnumerator Skill2Cooldown()
    {
        float radius = 3.0f;
        float summonTime = 0.3f;
        var WaitForSummon = new WaitForSeconds(summonTime);
        var WaitForCooltime = new WaitForSeconds(skill2Cooltime);
        
        while (true)
        {
            while(Com.MyAnim.GetBool(AnimParam.OnSkill1))
            {
                yield return null;
            }
            yield return new WaitForSeconds(5.0f); //선딜레이
            impList.Clear();
            AddStateChange(AdditionalState.Skill2);
            Com.MyAnim.SetBool(AnimParam.OnSkill2, true);
            for(int i = 0; i < 5; i++)
            {
                yield return WaitForSummon;
                IPoolable ImpInstance = ObjectPoolManager.Instance.GetObj(servantImp);
                impList.Add(ImpInstance);
                ImpInstance.This.enabled = false;
                //정면을 기준으로 한 반원에서 5개의 위치에 소환
                float angle = Mathf.PI * i / 4;
                Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
                pos = transform.rotation * pos;
                ImpInstance.This.transform.position = transform.position + pos;
            }
            yield return WaitForSummon;
            foreach (IPoolable imp in impList)
            {
                imp.This.enabled = true;
            }
            Com.MyAnim.SetBool(AnimParam.OnSkill2, false);
            AddStateChange(AdditionalState.None);
            yield return WaitForCooltime;
        }
    }

    private void AddStateChange(AdditionalState state)
    {
        if (addState == state) return;
        addState = state;
        switch (addState)
        {
            case AdditionalState.None:
                SetDirection(transform.forward);
                Com.MyAnim.SetBool(AnimParam.isMoving, true);
                break;
            case AdditionalState.Skill1:
                Com.MyAnim.SetBool(AnimParam.isMoving, false);
                break;
            case AdditionalState.Skill2:
                SetDirection(Vector3.zero);
                Com.MyAnim.SetBool(AnimParam.isMoving, false);
                break;
        }
    }
    #endregion


    #region StateProcess
    private IEnumerator DeathCoroutine()
    {
        SetDirection(Vector3.zero);
        col.enabled = false;
        Com.MyAnim.SetTrigger(AnimParam.Death);
        foreach (IPoolable imp in impList)
        {
            imp.This.enabled = true;
        }
        yield return new WaitForSeconds(1.0f);
        ObjectPoolManager.Instance.ReleaseObj(this);
        yield break;
    }

    protected override void ChangeState(State s)
    {
        if (myState == s) return;
        myState = s;
        switch (myState)
        {
            case State.Chase:
                Com.MyAnim.SetBool(AnimParam.isMoving, true);
                break;
            case State.Attack:
                StartCoroutine(Skill1CoolDown());
                break;
            case State.Death:
                StopAllCoroutines();
                StartCoroutine(DeathCoroutine());
                break;
        }
    }

    protected override void StateProcess()
    {
        if(addState != AdditionalState.None && myState != State.Death)
        {
            switch (addState)
            {
                case AdditionalState.Skill1:
                    SetDirection(transform.forward);
                    break;
                case AdditionalState.Skill2:
                    break;
            }
        }
        else
        {
            switch (myState)
            {
                case State.Chase:
                    SetDirection(transform.forward);
                    break;
                case State.Attack:
                    Vector3 dir = myTarget.position - transform.position;
                    if (dir.sqrMagnitude > BossData.AttackRange * BossData.AttackRange)
                    {
                        SetDirection(transform.forward);
                        Com.MyAnim.SetBool(AnimParam.isMoving, true);
                        Com.MyAnim.SetBool(AnimParam.isAttacking, false);
                    }
                    else
                    {
                        SetDirection(Vector3.zero);
                        Com.MyAnim.SetBool(AnimParam.isMoving,false);
                        Com.MyAnim.SetBool(AnimParam.isAttacking, true);
                    }
                    break;
                case State.Death:
                    SetDirection(Vector3.zero);
                    break;
            }
        }
    }
    #endregion



    bool leftAttack = false;
    public void OnAttackPoint()
    {
        Transform attackPoint = Com.AttackPoints[leftAttack ? 0 : 1];

        Collider[] cols = Physics.OverlapSphere(attackPoint.position, 0.5f, attackMask);

        foreach (var col in cols)
        {
            col.GetComponent<IDamage>()?.TakeDamage(Attack);
        }
        leftAttack = !leftAttack;
    }
}
