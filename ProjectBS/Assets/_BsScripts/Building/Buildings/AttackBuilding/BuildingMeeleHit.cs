using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BuildingMeeleHit : MonoBehaviour
{
    private short dmg;
    public float attackDelay;
    public LayerMask attackableLayer;
    private bool atkDelaying;
    private BoxCollider atkCollider;
    public UnityEvent setActiveEffectsEvent;


    // Start is called before the first frame update
    void OnEnable()
    {
        getParentBuildingAtkStats();
        atkCollider = GetComponent<BoxCollider>();
        
    }

    void getParentBuildingAtkStats()
    {
        AttackBuildingBase buildingStat = GetComponentInParent<AttackBuildingBase>(); //커플링. 부모 건물의 현재 공격력을 갖는다.
        dmg = buildingStat.SetDmg();
        attackDelay = buildingStat.SetAtkDelay();
        attackableLayer = buildingStat.SetAttackableMask();
    }

    protected virtual void OnTriggerStay(Collider other)
    {
        if (other != null && (1 << other.gameObject.layer & attackableLayer) != 0)
        {
            if (!atkDelaying)
            {
                atkDelaying = true;
                StartCoroutine(AtkDelay(attackDelay, other));
            }
        }

    }

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
    

    


    // Update is called once per frame
    void Update()
    {
        
    }
}
