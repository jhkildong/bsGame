using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class ProjectileEffectHit : MonoBehaviour , ISetProjectileStats
{
    ParticleSystem ps;

    public int ID => id;
    private float baseAttack; // 투사체 기본 공격력
    private float myProjectileSize; // 투사체 크기
    private float mySpeed; // 투사체 속도
    private float myRange; // 투사체 사거리
    private float curRange; // 이동한 거리
    [SerializeField] private int id; //공격 id
    [SerializeField] private bool canPenetrate; //관통 가능한 투사체인가?
    [SerializeField] private int orgPenetrateCount; // 관통가능한 횟수
    [SerializeField] private int curPenetrateCount; // 남은 관통횟수
    Vector3 oldPos;
    public LayerMask attackableLayer;

    public HitEffects hitEffect;
    public void SetProjectileStats(float atk = 1, float size = 1, float speed = 1, float range = 1, bool canPenetrate = false, int penetrateCount = 0) 
    {
        baseAttack = atk;
        myProjectileSize = size;
        mySpeed = speed;
        myRange = range;
        this.canPenetrate = canPenetrate;
        orgPenetrateCount = penetrateCount;

    }
    // Start is called before the first frame update

    void OnEnable()
    {
        curPenetrateCount = orgPenetrateCount;

    }

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        
        transform.Translate(Vector3.forward * mySpeed * Time.deltaTime);
        curRange +=  mySpeed * Time.deltaTime;

        if (curRange > myRange)
        {
            Debug.Log("최대 사거리 도달");
            curRange = 0f;

            //EffectPoolManager.Instance.ReleaseObject<ProjectileEffectHit>(gameObject);
            EffectPoolManager.Instance.ReleaseObject(gameObject,id);
        }
        /*
        Ray ray = new Ray(oldPos, (transform.position - oldPos).normalized);
        if (Physics.Raycast(ray, out RaycastHit hit, (transform.position - oldPos).magnitude, attackableLayer))
        {
            Hit(hit.transform.gameObject);
            Debug.Log("얼음공격");
            if(!canPenetrate) EffectPoolManager.Instance.ReleaseObject<ProjectileEffectHit>(gameObject); // 관통불가능한 경우
            else if (canPenetrate && orgPenetrateCount >0) // 관통가능한데, 관통횟수제한이 있는경우
            {
                curPenetrateCount--; //남은 관통횟수 차감
                if(curPenetrateCount < 0) EffectPoolManager.Instance.ReleaseObject<ProjectileEffectHit>(gameObject); // 남은 관통횟수가 0미만일때 poolmanager로 돌려보냄
            }
            
        }
        */
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != null && (attackableLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            Hit(other.gameObject);
            if (!canPenetrate) EffectPoolManager.Instance.ReleaseObject(gameObject, id); // 관통불가능한 경우 투사체 비활성화, 매니저로 반환
            else if (canPenetrate && orgPenetrateCount > 0) // 관통가능한데, 관통횟수제한이 있는경우
            {
                curPenetrateCount--; //남은 관통횟수 차감
                if (curPenetrateCount < 0) EffectPoolManager.Instance.ReleaseObject(gameObject, id); // 남은 관통횟수가 0미만일때 매니저로 반환
            }
        }
    }


    void Hit(GameObject hitTarget)
    {
        IDamage target = hitTarget.GetComponent<IDamage>();
        EffectPoolManager.Instance.SetActiveHitEffect(hitEffect, transform.position, hitEffect.ID); // 피격대상과 가장 가까운 점에 피격이펙트 생성
        target.TakeDamage(baseAttack);
        target.TakeDamageEffect(baseAttack);
    }






}
