using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class BuildingEffectHit : MonoBehaviour, ISetStats
{

    protected enum Type
    {
        Projectile,
        Point,
    }
    
    [SerializeField]protected Type atkType;

    ParticleSystem ps;

    public bool canHit;

    protected float hitTime = 1f; //���� Ÿ�̹�
    float progress; // ��ƼŬ ��� ���൵

    private short dmg;
    private short baseAttack;
    private float myRadius;
    private float myProjectileSize;

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
        Debug.Log("����");
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

    public void SetStats(short atk = 1, float radius = 1, float size = 1, float speed = 1) // �ǹ��� ������ EffectPoolManager�� ���� -> ����Ʈ �����ÿ� �ش� Stat�� ����Ʈ�� ����
    {
        baseAttack = atk;
        myRadius = radius;
        myProjectileSize = size;
    }

    void Update()
    {
        if(atkType == Type.Projectile)
        {
            
        }
        else if(atkType == Type.Point)
        {
            progress = ps.time / ps.main.duration;
            //if (Mathf.Approximately(progress, hitTime) && canHit)
            if((progress >hitTiming) && canHit)
            {
                canHit = false;
                Debug.Log("����!");
                HitSphere();
                
            }
            if(progress >= 1f)
            {
                //gameObject.SetActive(false);
                EffectPoolManager.Instance.ReleaseObject<BuildingEffectHit>(gameObject);
            }
        }

        
    }

    protected virtual void MeeleAttack()
    {

    }

    protected virtual void ProjectileAttack()
    {

    }

    protected virtual void PointAttack()
    {

    }

    protected void SetHitTiming(float timing)
    {
        hitTime = timing;

    }

    protected void HitSphere()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, myRadius,attackableLayer); 
        if(colliders.Length > 0 )
        {
            Debug.Log(colliders);
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
