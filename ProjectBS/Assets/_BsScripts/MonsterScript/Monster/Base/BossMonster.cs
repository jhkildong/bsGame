using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TheKiwiCoder;

public abstract class BossMonster : Monster
{
    public BossMonsterData BossData => _data as BossMonsterData;
    
    public List<Skill> SkillList;
    public bool isSkillCast
    {
        get => _isSkillCast;
        set
        {
            _isSkillCast = value;
        }
    }
    private bool _isSkillCast = false;

    protected BehaviourTreeRunner myTree;

    protected override void OnDisable()
    {
        base.OnDisable();
        myTree.enabled = false;
    }

    protected IEnumerator SkillCoolTime(int idx)
    {
        SkillList[idx].isReady = false;
        yield return new WaitForSeconds(SkillList[idx].skillCoolTime);
        SkillList[idx].isReady = true;
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if (((int)BSLayers.Building & (1 << collision.gameObject.layer)) != 0)
        {
            IDamage AttackTarget = collision.gameObject.GetComponent<IDamage>();
            if (AttackTarget != null)
            {
                myTarget = collision.transform;
            }
        }
    }

    protected override void OnCollisionExit(Collision collision)
    {
        if (((int)BSLayers.Building & (1 << collision.gameObject.layer)) != 0)
        {
            IDamage AttackTarget = collision.gameObject.GetComponent<IDamage>();
            if (AttackTarget != null)
            {
                myTarget = PlayerTransform;
            }
        }
    }
}

public class Skill
{
    public bool isReady;
    public float skillCoolTime;
    public bool isDuring;
    public event UnityAction skillAct;

    public Skill(UnityAction act, float cooltime)
    {
        isReady = true;
        isDuring = false;
        skillAct += act;
        skillCoolTime = cooltime;
    }

    public void reset()
    {
        isReady = true;
        isDuring = false;
    }

    public void OnSkill()
    {
        skillAct?.Invoke();
    }
}