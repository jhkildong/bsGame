using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class AttackBuildingBase : Building, IBuffable
{
    // Start is called before the first frame update

    public enum AtkType
    {
        Meele,
        Projectile,
        Area
    }
    [SerializeField] protected AtkType atkType;

    [SerializeField]protected GameObject target;
    protected List<Transform> detectedObj = new List<Transform>();
    public bool atkDelaying;

    public LayerMask attackableLayer;

    //public GameObject atkEffect;
    public List<GameObject> effectList; //

    public Transform effectPool; //������Ʈ Ǯ�� �� �ʱ� ��ġ. 

    public UnityEvent EffectPoolEvent;

    public UnityEvent AtkEvent; //AtkDelay ���� Invoke

   
    protected Vector3 relativeDir; // ����ü�� �߻��� ���⺤��

    public Buff getBuff { get; set; } = new Buff();
    

    /*
    protected short _atkPower;// ���ݰ����� �ǹ��� ���ݷ�
    public float _atkDelay;  // �ǹ��� ���� ���� ������
    protected float _hitDelay; // �ǹ� ������ Ÿ�� ���� (���� ������ ���)
    protected float _atkDuration; // �ǹ� ������ ���� �ð�( ���� ������ ���)
    protected float _atkRadius; // �ǹ��� ���� ������ (��ǥ ������ ����)
    protected float _atkProjectileSize; //�ǹ��� ����ü ������ (����ü�� ����)
    protected float _atkProjectileSpeed; // �ǹ� ����ü �ӵ�
    protected float _atkProjectileRange; // �ǹ� ����ü ��Ÿ�
    protected bool _atkCanPen; //���밡���� �����ΰ�?
    protected int _atkPenCount; //���밡���� ��ü��
    */
    /*0412 ������ ����
    protected override void Start()
    {
        base.Start();
        attackableLayer = Data.attackableLayer;
        _atkPower = Data.atkPower;
        _atkDelay = Data.atkDelay;
        _hitDelay = Data.hitDelay;
        _atkDuration = Data.atkDuration;
        _atkRadius = Data.atkRadius;
        _atkProjectileSize = Data.atkProjectileSize;
        _atkProjectileSpeed = Data.atkProjectileSpeed;
        _atkProjectileRange = Data.atkProjectileRange;
        _atkCanPen = Data.atkCanPen;
        _atkPenCount = Data.atkPenCount;
        Debug.Log("attackbuilding" + _constTime);
    }
    */

    protected override void Start()
    {
        base.Start();
        
    }

    public void SetAtkStats()
    {
        //���ȿ� ������ ������ ȣ��� �Լ�.

    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (iscompletedBuilding && atkType == AtkType.Area)
        {
            SetAttackTarget(other);
        }
        else if(iscompletedBuilding && atkType == AtkType.Projectile)
        {
            SetAttackTarget(other);
        }
        else if(iscompletedBuilding && atkType == AtkType.Meele)
        {
            //SetAttackTarget(other);
        }
    }

    protected void SetAttackTarget(Collider other) //���� ���� �����ȿ� ���� ���� Ÿ������
    {
        if ((1 << other.gameObject.layer & attackableLayer) != 0)
        {
            IDamage obj = other.GetComponent<IDamage>();
            if (obj != null)
            {
                detectedObj.Add((obj as Monster).transform);
                target = detectedObj[0].gameObject;
                //Debug.Log(target);

                (obj as Monster).DeadTransformAct += RemoveTarget; //Target�� ���� ã���̺�Ʈ�� ����صд�. Target�� �׾����� �̺�Ʈ �߻�.
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (iscompletedBuilding && (atkType == AtkType.Projectile || atkType == AtkType.Area) && (1 << other.gameObject.layer & attackableLayer) != 0)
        {
            IDamage obj = other.GetComponent<IDamage>();
            if (obj != null)
            {
                if (detectedObj.Contains((obj as Monster).transform))
                {
                    detectedObj.Remove((obj as Monster).transform);
                    if(detectedObj.Count > 0)
                    {
                        target = detectedObj[0].gameObject; //���ο� Ÿ�� ã��
                    }
                    else
                    {
                        target = null;
                    }
                    //Debug.Log(other.gameObject);
                }
            }
        }
    }

    void RemoveTarget(Transform tr)
    {
        foreach (Transform obj in detectedObj)
        {
            if(obj != null && obj.transform == tr)
            {
                detectedObj.Remove(obj);
                if (detectedObj.Count > 0)
                {
                    target = detectedObj[0].gameObject; //���ο� Ÿ�� ã��
                }
                else
                {
                    target = null;
                }
                return;
            }
        }
    }

    /* 0412 ������ ����
    protected override void Update() // ��ǻ� override �ƴ�. Building���� update�� �ϴ°� ����
    {
        base.Update();
        AttackToTarget();
    }

    protected virtual void AttackToTarget()
    {
        if (target != null&&!atkDelaying)
        {   
            atkDelaying = true;
            StartCoroutine(AtkDelay(_atkDelay));
        }
    }

    protected virtual IEnumerator AtkDelay(float delay)
    {
        Debug.Log("����!");
        //���⸦ �̺�Ʈ�� ȣ���� �Լ��� �߰��ؾߵ�.
        AtkEvent?.Invoke();
        yield return new WaitForSeconds(delay);
        atkDelaying = false;
    }


    protected override void ConstructComplete()
    {
        base.ConstructComplete();
        //InstEffects();
    }
    */

    protected void InstEffects() // ����Ʈ ������Ʈ Ǯ�� (����)
    {
        /*
        GameObject eft = Instantiate(atkEffect, effectPool);
        effectList.Add(eft);
        eft.SetActive(false);
        */

    }




    /*
    protected void OnMeeleAttack(Collider other)
    {
        if ((1 << other.gameObject.layer & attackableLayer) != 0)
        {
            IDamage obj = other.GetComponent<IDamage>();
            if (obj != null)
            {
                obj.TakeDamage(_attackPower);
                //Debug.Log(obj);
            }
        }
    }
    */


    /*0412 ������
    public short SetDmg()
    {
        return _atkPower;
    }
    public float SetAtkRadius()
    {
        return _atkRadius;
    }
    public float SetAtkProjectileSize()
    {
        return _atkProjectileSize;
    }
    public float SetAtkDelay()
    {
        return _atkDelay;
    }
    */

}
