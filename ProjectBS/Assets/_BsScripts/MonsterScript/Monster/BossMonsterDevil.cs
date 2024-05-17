using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BossMonsterDevil : BossMonster
{
    protected override void InitCollider(float radius)
    {
        col = gameObject.AddComponent<CapsuleCollider>();
        (col as CapsuleCollider).radius = radius;
        (col as CapsuleCollider).height = 2.5f;
        (col as CapsuleCollider).center = new Vector3(0, 1.25f, 0);
    }

    public override void AttackTarget()
    {
        if (attackTarget == null) return;

        SetDirection(Vector3.zero);
        Com.MyAnim.SetTrigger(AnimParam.Attack);
    }

    public override bool AttackReady()
    {
        return attackReady;
    }

    public override bool AttackTargetInRange()
    {
        if (Vector3.SqrMagnitude(myTarget.position - transform.position) < BossData.AttackRange * BossData.AttackRange)
        {
            attackTarget = myTarget.GetComponent<IDamage>();
        }
        else
        {
            attackTarget = null;
        }
        return base.AttackTargetInRange();
    }

    public override void Init(MonsterData data)
    {
        base.Init(data);
        BehaviourTree tree = Resources.Load<BehaviourTree>(FilePath.DevilBehaviourTree);
        myTree = GetComponent<BehaviourTreeRunner>();
        myTree.Init(tree);
        MonsterData servantImpData = Resources.Load<MonsterData>(FilePath.Imp);
        servantImp = servantImpData.CreateClone();
        ObjectPoolManager.Instance.ReleaseObj(servantImp);
        Com.MyAnimEvent.AttackAct += OnAttackPoint;

        SkillList = new List<Skill>
        {
            new Skill(OnSkill0, skill0Cooltime),
            new Skill(OnSkill1, skill1Cooltime)
        };
    }

    private Monster servantImp;
    private float skill0Cooltime = 120.0f;
    private float skill1Cooltime = 20.0f;
    private List<IPoolable> impList = new List<IPoolable>();    //소환 하는 도중에 죽는 경우 소환된 인스턴스들을 실행시켜 주기 위해 멤버변수로 관리
    private Coroutine skill0Coroutine;
    private Coroutine skill1Coroutine;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (Data == null)
            return;
        foreach (var skill in SkillList)
        {
            skill.reset();
        }
        myTree.enabled = true;
        col.enabled = true;
    }

    protected override void Die()
    {
        
        if(skill0Coroutine != null) StopCoroutine(skill0Coroutine);
        if(skill1Coroutine != null) StopCoroutine(skill1Coroutine);
        StartCoroutine(DeathCoroutine());
    }

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
        GameObject go = ItemManager.Instance.DropRandomItem(Data.DropItemList);
        if (go != null)
            go.transform.position = transform.position + Vector3.up * 0.7f + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        GameObject exp = ItemManager.Instance.DropExp(Data.Exp);
        if (exp != null)
            exp.transform.position = transform.position + Vector3.up * 0.7f + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        GameObject gold = ItemManager.Instance.DropGold(Data.Gold);
        if (gold != null)
            gold.transform.position = transform.position + Vector3.up * 0.7f + new Vector3(Random.Range(-1.0f, 1.0f), 0, Random.Range(-1.0f, 1.0f));
        ObjectPoolManager.Instance.ReleaseObj(this);
    }

    private void OnSkill0()
    {
        skill0Coroutine = StartCoroutine(Skill0SummonImp());
    }

    private IEnumerator Skill0SummonImp()
    {
        int idx = 0;
        float radius = 3.0f;
        float summonTime = 0.3f;
        var WaitForSummon = new WaitForSeconds(summonTime);
        
        Com.MyAnim.SetBool(AnimParam.OnSkill0, true);
        SkillList[idx].isDuring = true;

        impList.Clear();
        for (int i = 0; i < 5; i++)
        {
            yield return WaitForSummon; // 소환 대기시간
            IPoolable ImpInstance = ObjectPoolManager.Instance.GetObj(servantImp);
            impList.Add(ImpInstance);
            ImpInstance.This.enabled = false;
            //정면을 기준으로 한 반원에서 5개의 위치에 소환
            float angle = Mathf.PI * i / 4;
            Vector3 pos = new Vector3(radius * Mathf.Cos(angle), 0, radius * Mathf.Sin(angle));
            pos = transform.rotation * pos;
            ImpInstance.This.transform.position = transform.position + pos;
        }
        yield return WaitForSummon; // 소환이 끝나고 나서 대기 시간
        foreach (IPoolable imp in impList)
        {
            imp.This.enabled = true;
        }
        Com.MyAnim.SetBool(AnimParam.OnSkill0, false);
        SkillList[idx].isDuring = false;
        StartCoroutine(SkillCoolTime(idx));
    }


    private void OnSkill1()
    {
        skill1Coroutine = StartCoroutine(Skill1Whirlwind());
    }


    private IEnumerator Skill1Whirlwind()
    {
        int idx = 1;
        int attackCount = 5;
        float attackDelay = 0.4f;
        var WaitForAttackDelay = new WaitForSeconds(attackDelay);

        moveSpeed = Data.Sp * 2f;
        Com.MyAnim.SetBool(AnimParam.OnSkill1, true);
        SkillList[idx].isDuring = true;
        
        for(int i = 0; i < attackCount; i++)
        {
            Collider[] cols = Physics.OverlapSphere(transform.position + new Vector3(0, 1.0f ,0), 2.0f, attackMask);
            foreach (Collider col in cols)
            {
                col.GetComponent<IDamage>()?.TakeDamageEffect(Attack);
            }
            yield return WaitForAttackDelay;
        }
        moveSpeed = Data.Sp;
        Com.MyAnim.SetBool(AnimParam.OnSkill1, false);
        SkillList[idx].isDuring = false;
        StartCoroutine(SkillCoolTime(idx));
    }


    bool leftAttack = false;
    public void OnAttackPoint()
    {
        Transform attackPoint = Com.AttackPoints[leftAttack ? 0 : 1];

        Collider[] cols = Physics.OverlapSphere(attackPoint.position, 0.5f, attackMask);

        foreach (var col in cols)
        {
            col.GetComponent<IDamage>()?.TakeDamageEffect(Attack);
        }
        leftAttack = !leftAttack;
    }
    
}
