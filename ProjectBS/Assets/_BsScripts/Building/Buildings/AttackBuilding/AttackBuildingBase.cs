using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;


public class AttackBuildingBase : Building
{
    // Start is called before the first frame update

    public enum AtkType
    {
        Meele,
        Projectile,
        Point
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

    protected short _attackPower;// ���ݰ����� �ǹ��� ���ݷ�
    public float _attackDelay;  // �ǹ��� ���� ������
    protected float _attackRadius; // �ǹ��� ���� ������ (��ǥ ������ ����)
    protected float _attackProjectileSize; //�ǹ��� ����ü ������ (����ü�� ����)
//    protected Vector3 _attackBoxSize;
    /*
    public short Damage
    {
        get { return _attackPower; }
        set { _attackPower = value; }
    }
    */
    protected override void Start()
    {
        base.Start();
        _attackPower = Data.attackPower;
        _attackDelay = Data.attackDelay;
        _attackRadius = Data.attackRadius;
        _attackProjectileSize = Data.attackProjectileSize;
        Debug.Log("attackbuilding" + _constTime);
    }

    public void SetAtkStats()
    {
        //���ȿ� ������ ������ ȣ��� �Լ�.

    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        if (iscompletedBuilding && (atkType == AtkType.Projectile || atkType == AtkType.Point))
        {
            SetAttackTarget(other);
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
        if (iscompletedBuilding && (atkType == AtkType.Projectile || atkType == AtkType.Point) && (1 << other.gameObject.layer & attackableLayer) != 0)
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


    protected override void Update() // ��ǻ� override �ƴ�. Building���� update�� �ϴ°� ����
    {
        base.Update();
        AttackToTarget();
    }

    protected virtual void AttackToTarget()
    {
        if (target != null)
        {
            if (!atkDelaying)
            {
                atkDelaying = true;
                StartCoroutine(AtkDelay(_attackDelay));
            }
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


    public LayerMask SetAttackableMask()
    {
        return attackableLayer;
    }
    public short SetDmg()
    {
        return _attackPower;
    }
    public float SetAtkRadius()
    {
        return _attackRadius;
    }
    public float SetAtkProjectileSize()
    {
        return _attackProjectileSize;
    }
    public float SetAtkDelay()
    {
        return _attackDelay;
    }


}
