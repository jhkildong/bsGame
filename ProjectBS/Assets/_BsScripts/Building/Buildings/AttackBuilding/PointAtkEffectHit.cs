using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PointAtkEffectHit : MonoBehaviour, ISetPointStats
{

    protected enum Type
    {
        Once,
        Last
    }
    
    [SerializeField]protected Type atkType;

    ParticleSystem ps;

    public bool canHit;

    protected float hitTime = 1f; //���� Ÿ�̹�
    float progress; // ��ƼŬ ��� ���൵

    private float dmg;
    private float baseAttack;
    private float myRadius;
    private float atkDelay; // ���� ����
    private float atkDuration; // ���ӽð�
    private float curDur; //���� ���ӽð�
    private bool atkDelaying;

    [SerializeField]protected float hitTiming; // Ÿ�� Ÿ�̹� (0~1����)

    public LayerMask attackableLayer;

    private void Awake()
    {
       
    }
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }
    void OnEnable()
    {
        //getParentBuildingAtkStats();
        canHit = true;
        curDur = 0f;
    }
    /*
    void getParentBuildingAtkStats()
    {
        
        AttackBuildingBase buildingStat = GetComponentInParent<AttackBuildingBase>(); //Ŀ�ø�. �θ� �ǹ��� ���� ���ݷ��� ���´�.
        Debug.Log(buildingStat);
        Debug.Log("������ " + dmg);
        dmg = buildingStat.SetDmg();
        myRadius = buildingStat.SetAtkRadius();
        myProjectileSize = buildingStat.SetAtkProjectileSize();

    }
    */

    public void SetPointStats(float atk = 1, float radius = 1, float size = 1, float speed = 1,float delay = 1, float durTime = 1) // �ǹ��� ������ EffectPoolManager�� ���� -> ����Ʈ �����ÿ� �ش� Stat�� ����Ʈ�� ����
    {
        baseAttack = atk;
        myRadius = radius;
        atkDelay = delay;
        atkDuration = durTime;
    }

    void Update()
    {

        if (atkType == Type.Once) // 1ȸ Ÿ�� ������ ���
        {
            progress = ps.time / ps.main.duration;
            //if (Mathf.Approximately(progress, hitTime) && canHit)
            if ((progress > hitTiming) && canHit)
            {
                canHit = false;
                HitSphere();
            }
            if (progress >= 1f)
            {
                //gameObject.SetActive(false);
                EffectPoolManager.Instance.ReleaseObject<PointAtkEffectHit>(gameObject);
            }
        }

        else if (atkType == Type.Last) // ���Ӱ����� ��� ( ���Ǳ� )
        {
             progress = ps.time / ps.main.duration;
             curDur += Time.deltaTime;
            //���ӽð����� �ݺ��ؼ� �����̸��� ����.
            if (!atkDelaying && (progress > hitTiming) && curDur < atkDuration)
            {
                atkDelaying = true;
                StartCoroutine(LastAtk(atkDelay));
            }
            else if(curDur >= atkDuration) // ���ӽð��� ������ Ǯ�� �ǵ���
            {       
                atkDelaying = false;
                EffectPoolManager.Instance.ReleaseObject<PointAtkEffectHit>(gameObject);
            }
        }

    }

    IEnumerator LastAtk(float delay) //Ÿ�� �ð������� �����ϱ� ���� �ڷ�ƾ �Լ�
    {
        HitSphere();
        yield return new WaitForSeconds(delay);
        atkDelaying = false;
    }


    protected void SetHitTiming(float timing)
    {
        hitTime = timing;

    }

    protected void HitSphere()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, myRadius, attackableLayer); 
        if(colliders.Length > 0 )
        {
            //Debug.Log(colliders);
        }

        foreach (Collider collider in colliders)
        {
            IDamage target = collider.GetComponent<IDamage>();
            target.TakeDamage(baseAttack);
        }
        //�������� ������ (Idamage �� �ִ�)
        //������ ����
    }

}
