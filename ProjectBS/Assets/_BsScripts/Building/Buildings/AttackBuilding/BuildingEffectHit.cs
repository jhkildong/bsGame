using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingEffectHit : MonoBehaviour
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
    private float radius;
    private float projectileSize;

    public LayerMask attackableLayer;

    public UnityAction getAtkStat;
    private void Awake()
    {
        /*
        //��ϵ� ���ݷ��� �������� �̺�Ʈ�� ��ϵȰ� ������ �̺�Ʈ �߰��ϰ�, �̺�Ʈ ����.
        if(getAtkStat == null)
        {
            AttackBuildingBase buildingStat = GetComponentInParent<AttackBuildingBase>();
            dmg = buildingStat.SetDmg();
            radius = buildingStat.SetAtkRadius();
        }
        */
    }
    void Start()
    {
        ps = GetComponent<ParticleSystem>();


    }
    void OnEnable()
    {
        getParentBuildingAtkStats();
        canHit = true;
        Debug.Log("����");
    }

    void getParentBuildingAtkStats()
    {
        
        AttackBuildingBase buildingStat = GetComponentInParent<AttackBuildingBase>(); //Ŀ�ø�. �θ� �ǹ��� ���� ���ݷ��� ���´�.
        dmg = buildingStat.SetDmg();
        radius = buildingStat.SetAtkRadius();
        projectileSize = buildingStat.SetAtkProjectileSize();

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
            if((progress >0.3f) && canHit)
            {
                canHit = false;
                Debug.Log("����!");
                HitSphere();
                
            }
            if(progress >= 1f)
            {
                gameObject.SetActive(false);    
            }
        }

        
    }

    public void SetAtkStats()
    {
        
    }

    protected void SetHitTiming(float timing)
    {
        hitTime = timing;

    }

    protected void HitSphere()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, radius,attackableLayer); 
        if(colliders.Length > 0 )
        {
            Debug.Log(colliders);
        }

        foreach (Collider collider in colliders)
        {
            IDamage target = collider.GetComponent<IDamage>();
            target.TakeDamage(dmg);
            /*
            if(target != null && ((collider.gameObject.layer&attackableLayer)!= 0))
            {
                target.TakeDamage(dmg);
            }
            
            else
            {
                Debug.Log("�νĵǴ� Ÿ���� �����");
            }
            */
        }
        //�������� ������ (Idamage �� �ִ�)
        //������ ����
    }

}
