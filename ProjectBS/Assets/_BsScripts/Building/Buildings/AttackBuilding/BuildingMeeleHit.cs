using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class BuildingMeeleHit : MonoBehaviour//, ISetMeeleStats
{
    //이 클래스가 하는일. 닿은 오브젝트 검사. 물체가 감지되면, 이펙트 호출.
    //근접 공격 유형 -> 주기마다,키입력시 해당 위치에 생성,타이밍에 공격
    //               -> 생성 되어있는 상태로 지속. 충돌시 타격.
    //근접 공격 로직 -> 스탯을 받아와서, 위치에 생성. 타이밍에 공격.
    ParticleSystem ps;

    [SerializeField]List<Transform> detectedObj = new List<Transform>();

    public bool canHit;

    protected float hitTime = 1f; //공격 타이밍
    float progress; // 파티클 재생 진행도

    [SerializeField] protected float _finalDmg; // 최종데미지
    private float damage;
    private float mySize;
    private Vector3 myColSize;
    private float myAtkDelay;
    private bool atkDelaying;
    public LayerMask attackableLayer;

    [SerializeField]private HitEffects hitEffect;
    /*
    public void SetMeeleStats(short atk = 1, float radius = 1, float size = 1, float speed = 1, float atkDelay = 1)
    {
        baseAttack = atk;
        mySize = size;
        myAtkDelay = atkDelay; 
    }
    */
    public Buff getBuff { get; set; } = new Buff();


    void OnEnable()
    {
        getParentBuildingAtkStats();
    }

    void getParentBuildingAtkStats()
    {
        /* 0412 수정전
        AttackBuildingBase buildingStat = GetComponentInParent<AttackBuildingBase>();
        baseAttack = buildingStat.SetDmg();
        myAtkDelay = buildingStat.SetAtkDelay();
        attackableLayer = buildingStat.SetAttackableMask();
        */
        AttackBuilding_Meele buildingStat = GetComponentInParent<AttackBuilding_Meele>();
        damage = buildingStat.SetDmg();
        myAtkDelay = buildingStat.SetHitDelay();
        attackableLayer = buildingStat.SetAttackableMask();

    }
    void Start()
    {
        ps = GetComponent<ParticleSystem>();
        myColSize = GetComponent<Collider>().bounds.size;

        

    }
    void Update()
    {
        if(!atkDelaying)
        {
            atkDelaying = true;
            StartCoroutine(AtkDelay(myAtkDelay));
        }
    }

    IEnumerator AtkDelay(float delay)
    {
        getParentBuildingAtkStats(); // 공격시마다 스탯 갱신 ( 방식 수정 필요 )
        //여기서 공격
        Collider[] colliders = Physics.OverlapBox(transform.position, myColSize, Quaternion.identity, attackableLayer);
        Debug.Log(myColSize);
        Debug.Log(delay);
        foreach (Collider collider in colliders)
        {
            IDamage target = collider.GetComponent<IDamage>();
            target.TakeDamage(damage);
            Vector3 contact = collider.ClosestPoint(transform.position); // 충돌한 위치와 가장 가까운 점을 찾는다.
            EffectPoolManager.Instance.SetActiveHitEffect(hitEffect, contact, hitEffect.ID); // 피격대상과 가장 가까운 점에 피격이펙트 생성
        }
        yield return new WaitForSeconds(delay);
        atkDelaying = false;

    }

    /*
    protected IEnumerator AtkDelay(float delay, Collider other)
    {

        Debug.Log("공격!");
        MeeleAtk();
        //SetActiveEffects(other);
        //이 이벤트가 발생 했을때, AttackBuilding_Meele에 이벤트 발생을 알리고, AttackBuilding_Meele에서는 이벤트 발생했을때, 이펙트를 SetActiveEffects 해라.
        yield return new WaitForSeconds(delay);
        atkDelaying = false;
    }

    protected void MeeleAttack()
    {
        //SetActiveMeeleAtkEffect();
        MeeleAtk();
    }


    public void MeeleAtk()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, atkCollider.size, transform.rotation, attackableLayer);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                IDamage target = collider.GetComponent<IDamage>();
                if (target != null)
                {
                    target.TakeDamage(dmg);
                    Debug.Log("근접 건물 공격!");
                    //SetActiveMeeleAtkEffect(collider);
                }

            }
        }

    }
    */

}
