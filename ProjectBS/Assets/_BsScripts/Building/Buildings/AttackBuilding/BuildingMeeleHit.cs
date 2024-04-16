using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.GraphicsBuffer;

public class BuildingMeeleHit : MonoBehaviour//, ISetMeeleStats
{
    //�� Ŭ������ �ϴ���. ���� ������Ʈ �˻�. ��ü�� �����Ǹ�, ����Ʈ ȣ��.
    //���� ���� ���� -> �ֱ⸶��,Ű�Է½� �ش� ��ġ�� ����,Ÿ�ֿ̹� ����
    //               -> ���� �Ǿ��ִ� ���·� ����. �浹�� Ÿ��.
    //���� ���� ���� -> ������ �޾ƿͼ�, ��ġ�� ����. Ÿ�ֿ̹� ����.
    ParticleSystem ps;

    [SerializeField]List<Transform> detectedObj = new List<Transform>();

    public bool canHit;

    protected float hitTime = 1f; //���� Ÿ�̹�
    float progress; // ��ƼŬ ��� ���൵

    private short baseAttack;
    private float mySize;
    private Vector3 myColSize;
    private float myAtkDelay;
    private bool atkDelaying;
    public LayerMask attackableLayer;
    /*
    public void SetMeeleStats(short atk = 1, float radius = 1, float size = 1, float speed = 1, float atkDelay = 1)
    {
        baseAttack = atk;
        mySize = size;
        myAtkDelay = atkDelay; 
    }
    */



    void OnEnable()
    {
        getParentBuildingAtkStats();
    }

    void getParentBuildingAtkStats()
    {
        /* 0412 ������
        AttackBuildingBase buildingStat = GetComponentInParent<AttackBuildingBase>();
        baseAttack = buildingStat.SetDmg();
        myAtkDelay = buildingStat.SetAtkDelay();
        attackableLayer = buildingStat.SetAttackableMask();
        */
        AttackBuilding_Meele buildingStat = GetComponentInParent<AttackBuilding_Meele>();
        baseAttack = buildingStat.SetDmg();
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
        //���⼭ ����
        Collider[] colliders = Physics.OverlapBox(transform.position, myColSize, Quaternion.identity, attackableLayer);
        Debug.Log(myColSize);
        Debug.Log(delay);
        foreach (Collider collider in colliders)
        {
            IDamage target = collider.GetComponent<IDamage>();
            target.TakeDamage(baseAttack);
        }
        yield return new WaitForSeconds(delay);
        atkDelaying = false;

    }

    /*
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
    */

}
