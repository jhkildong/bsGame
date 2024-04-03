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
        AttackBuildingBase buildingStat = GetComponentInParent<AttackBuildingBase>(); //Ŀ�ø�. �θ� �ǹ��� ���� ���ݷ��� ���´�.
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
        Debug.Log("����!");
        MeeleAtk();
        //SetActiveEffects(other);
        //�� �̺�Ʈ�� �߻� ������, AttackBuilding_Meele�� �̺�Ʈ �߻��� �˸���, AttackBuilding_Meele������ �̺�Ʈ �߻�������, ����Ʈ�� SetActiveEffects �ض�.
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
                    Debug.Log("���� �ǹ� ����!");
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
