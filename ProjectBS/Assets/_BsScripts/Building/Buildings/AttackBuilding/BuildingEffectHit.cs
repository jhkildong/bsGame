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

    protected float hitTime = 1f; //공격 타이밍
    float progress; // 파티클 재생 진행도

    private short dmg;
    private float radius;
    private float projectileSize;

    public LayerMask attackableLayer;

    public UnityAction getAtkStat;
    private void Awake()
    {
        /*
        //등록된 공격력을 가져오는 이벤트에 등록된게 없으면 이벤트 추가하고, 이벤트 실행.
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
        Debug.Log("켜짐");
    }

    void getParentBuildingAtkStats()
    {
        
        AttackBuildingBase buildingStat = GetComponentInParent<AttackBuildingBase>(); //커플링. 부모 건물의 현재 공격력을 갖는다.
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
                Debug.Log("지금!");
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
                Debug.Log("인식되는 타겟이 없어요");
            }
            */
        }
        //범위내의 적에게 (Idamage 가 있는)
        //데미지 전달
    }

}
