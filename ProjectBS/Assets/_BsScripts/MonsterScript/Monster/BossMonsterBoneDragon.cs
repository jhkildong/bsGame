using System.Collections;
using System.Collections.Generic;
using TheKiwiCoder;
using UnityEngine;

public class BossMonsterBoneDragon : BossMonster
{
    DragonComponent myCom => Com as DragonComponent;

    public override bool AttackReady()
    {
        return attackReady;
    }

    public override bool AttackTargetInRange()
    {
        if (Vector3.SqrMagnitude(myTarget.position - transform.position) < BossData.AttackRange * BossData.AttackRange)
        {
            if (Vector3.Angle(transform.forward, myTarget.position - transform.position) <= 45.0f)
                return true;
        }
        return false;
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
        Com.MyAnimEvent.EffectAct += EffectPlay;
        myCom.DragonFire.Attack = Data.Ak;
    }

    private void EffectPlay(int i)
    {
        if (i == 0)
        {
            myCom.MyEffect.Play();
            myCom.DragonFire.StartEffect();
        }
        else
        {
            myCom.MyEffect.Stop();
            myCom.DragonFire.StopEffect();
        }
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

        if (nextPhase)
        {
            nextPhase = false;
            isChnagePhase = true;
            Com.MyAnim.SetTrigger(AnimParam.Reassemble);
            while (isChnagePhase)
                yield return null;
            CurHp = Data.MaxHP * 1.5f;
            yield return new WaitForSeconds(0.5f);
            col.enabled = true;
            SkillList[1].isReady = true;
            SkillList[2].isReady = true;
            foreach (var skill in SkillList)
            {
                skill.reset();
            }
            SetDirection(transform.forward);
            Com.MyAnim.SetBool(AnimParam.isMoving, true);
            yield break;
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
        col.isTrigger = true;
        (col as SphereCollider).radius = 4.0f;
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
        col.isTrigger = false;
        (col as SphereCollider).radius = 2.5f;
        StartCoroutine(SkillCoolTime(idx));
    }

    protected override void Update()
    {
        if (isSkillCast) return;
        base.Update();
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if ((attackMask & (1 << collision.gameObject.layer)) != 0)
        {
            IDamage AttackTarget = collision.gameObject.GetComponent<IDamage>();
            if (AttackTarget != null)
            {
                attackTarget = AttackTarget;
                playTimer();
            }
        }
    }

    protected override void OnCollisionExit(Collision collision)
    {
        if ((attackMask & (1 << collision.gameObject.layer)) != 0)
        {
            IDamage AttackTarget = collision.gameObject.GetComponent<IDamage>();
            if (AttackTarget == attackTarget)
            {
                attackTarget = null;
                stopTimer();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((attackMask & (1 << other.gameObject.layer)) != 0)
        {
            IDamage AttackTarget = other.gameObject.GetComponent<IDamage>();
            if (AttackTarget != null)
            {
                AttackTarget.TakeDamageEffect(Data.Ak*2.0f);
            }
        }
    }
}
