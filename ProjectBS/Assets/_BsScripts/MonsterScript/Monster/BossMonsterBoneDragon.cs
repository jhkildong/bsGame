using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;

public class BossMonsterBoneDragon : BossMonster
{
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
        BehaviourTree tree = Resources.Load<BehaviourTree>(FilePath.BoneDragonBehaviourTree);
        myTree = GetComponent<BehaviourTreeRunner>();
        myTree.Init(tree);

        SkillList = new List<Skill>
        {
            new Skill(OnSkill0, skill0Cooltime),
            new Skill(OnSkill1, skill1Cooltime),
            new Skill(OnSkill2, skill2Cooltime)
        };
    }

    private bool nextPhase = true;
    private float skill0Cooltime = 20.0f;
    private float skill1Cooltime = 30.0f;
    private float skill2Cooltime = 40.0f;
    private Coroutine skill0Coroutine;
    private Coroutine skill1Coroutine;
    private Coroutine skill2Coroutine;

    protected override void OnEnable()
    {
        base.OnEnable();
        Init(Data);

        if (Data == null)
            return;
        foreach (var skill in SkillList)
        {
            skill.reset();
        }
        SkillList[1].isReady = false;
        SkillList[2].isReady = false;
        myTree.enabled = true;
        col.enabled = true;
        Com.MyAnim.SetBool(AnimParam.isMoving, true);
    }

    protected override void Die()
    {
        if (skill0Coroutine != null) StopCoroutine(skill0Coroutine);
        if (skill1Coroutine != null) StopCoroutine(skill1Coroutine);
        if (skill2Coroutine != null) StopCoroutine(skill2Coroutine);


        StartCoroutine(DeathCoroutine());
    }

    private IEnumerator DeathCoroutine()
    {
        SetDirection(Vector3.zero);
        col.enabled = false;
        Com.MyAnim.SetTrigger(AnimParam.Death);
        
        if(nextPhase)
        {
            nextPhase = false;
            isChnagePhase = true;
            Com.MyAnim.SetTrigger(AnimParam.Reassemble);
            while (isChnagePhase)
                yield return null;
            yield return new WaitForSeconds(0.5f);
            SkillList[1].isReady = true;
            SkillList[2].isReady = true;
            col.enabled = true;
            CurHp = Data.MaxHP * 1.5f;
            foreach (var skill in SkillList)
            {
                skill.reset();
            }
            SetDirection(transform.forward);
            Com.MyAnim.SetBool(AnimParam.isMoving, true);

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
        skill0Coroutine = StartCoroutine(Skill0BreathFire());
    }

    private IEnumerator Skill0BreathFire()
    {
        int idx = 0;

        SkillList[idx].isDuring = true;
        isSkillCast = true;
        Com.MyAnim.SetBool(AnimParam.isMoving, false);
        Com.MyAnim.SetTrigger(AnimParam.OnSkill0);
        while (isSkillCast)
        {
            yield return null;
        }

        Com.MyAnim.SetBool(AnimParam.isMoving, true);
        SkillList[idx].isDuring = false;
        skill0Coroutine = null;
        StartCoroutine(SkillCoolTime(idx));
    }


    private void OnSkill1()
    {
        skill1Coroutine = StartCoroutine(Skill1FlyFireBreath());
    }


    private IEnumerator Skill1FlyFireBreath()
    {
        int idx = 1;

        float height = 5.0f;
        SkillList[idx].isDuring = true;
        isSkillCast = true;
        Com.MyAnim.SetBool(AnimParam.isMoving, false);
        Com.MyAnim.SetBool(AnimParam.isFly, true);
        while (true)
        {
            transform.Translate(Vector3.up * (height * Time.deltaTime * 0.5f), Space.World);
            if (transform.position.y >= height)
            {
                break;
            }
            yield return null;
        }

        Com.MyAnim.SetTrigger(AnimParam.OnSkill1);
        while (isSkillCast)
        {
            yield return null;
        }

        while (transform.position.y >= 0)
        {

            transform.Translate(Vector3.down * (height * Time.deltaTime * 0.5f), Space.World);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        Com.MyAnim.SetBool(AnimParam.isFly, false);
        Com.MyAnim.SetBool(AnimParam.isMoving, true);
        SkillList[idx].isDuring = false;
        skill1Coroutine = null;
        StartCoroutine(SkillCoolTime(idx));
    }


    private void OnSkill2()
    {
        skill2Coroutine = StartCoroutine(Skill2FlyDiveAttack());
    }


    private IEnumerator Skill2FlyDiveAttack()
    {
        int idx = 2;
        float height = 13.0f;
        float back = 6.0f;
        col.isTrigger = false;
        SkillList[idx].isDuring = true;
        isSkillCast = true;
        Com.MyAnim.SetBool(AnimParam.isMoving, false);
        Com.MyAnim.SetBool(AnimParam.isFly, true);
        while (true)
        {
            transform.Translate(Vector3.up * (height * Time.deltaTime * 0.5f), Space.World);
            transform.Translate(Vector3.back * (back * Time.deltaTime), Space.Self);
            if (transform.position.y >= height)
            {
                break;
            }
            yield return null;
        }
        Com.MyAnim.SetTrigger(AnimParam.OnSkill2);
        Com.RootMotion.isStop = false;
        while (isSkillCast)
        {
            yield return null;
        }
        Com.RootMotion.isStop = true;
        while (transform.position.y >= 0)
        {
            transform.Translate(Vector3.down, Space.World);
            yield return null;
        }
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
        Com.MyAnim.SetBool(AnimParam.isFly, false);
        Com.MyAnim.SetBool(AnimParam.isMoving, true);

        SkillList[idx].isDuring = false;
        skill2Coroutine = null;
        col.isTrigger = true;
        StartCoroutine(SkillCoolTime(idx));
    }

    protected override void Update()
    {
        if (isSkillCast) return;
        base.Update();
    }

}
