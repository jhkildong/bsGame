using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBuilding_Buff : BuffBuildingBase , IBuffController
{

    [SerializeField] private BuffBuildingData BuffBuildingData;
    public BuffBuildingData BData
    {
        get { return BuffBuildingData; }
        set { BuffBuildingData = value; }
    }

    Dictionary<string, float> buffType = null;
    /*
    [SerializeField] private string buffName;
    [SerializeField] private float buffAmount;
    [SerializeField] private bool hasDuration;
    [SerializeField] private float duration;
    [SerializeField] private bool canStack;
    */
    

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        targetLayer = BData.targetLayer;
        
        /*
        buffName = BData.buffName;
        buffAmount = BData.buffAmount;
        hasDuration = BData.hasDuration;
        duration = BData.duration;
        canStack = BData.canStack;
        */
        buffController = new BuffController(this, BData.buffName, BData.buffAmount, BData.hasDuration, BData.duration, BData.canStack);

    }

    public BuffController buffController { get; set; }

    
    
    protected override void StartBuff(Collider other) // ontriggerenter��
    {
        IBuffable buffable = other.GetComponent<IBuffable>();
        if (buffable != null)
        {
            Buff buff = buffable.getBuff; // ������ ���� ��������
            if (buff == null)
            {
                buff = new Buff(); // ������ ������ ���� ����
            }
            switch (BData.buffType)
            {
                case BuffBuildingData.BuffType.Heal:
                    // ó���� ����
                    break;
                case BuffBuildingData.BuffType.atkBuff:
                    buffType = buff.atkBuffDict;
                    break;
                case BuffBuildingData.BuffType.hpBuff:
                    buffType = buff.hpBuffDict;
                    break;
                case BuffBuildingData.BuffType.asBuff:
                    buffType = buff.asBuffDict;
                    break;
                case BuffBuildingData.BuffType.msBuff:
                    buffType = buff.msBuffDict;
                    break;
                case BuffBuildingData.BuffType.rangeBuff:
                    buffType = buff.rangeBuffDict;
                    break;
                default:
                    break;
            }

            buffController.StartBuff(buffable, buffType);
        } 

        /*
        IBuffable buffable = other.GetComponent<IBuffable>();
        Debug.Log(buffable);
        if (buffable != null)
        {
            Buff buff = buffable.getBuff; // ������ ���� ��������
            if (buff == null)
            {
                buff = new Buff(); // ������ ������ ���� ����
            }
            // ���� �� ����
            //buff.atkBuffList.Add(buffAmount);
            if(canStack) // ��ø�� ������ ���
            {
                if (hasDuration) // ���ӽð��� �ִ� ������ ���
                {
                    while (true)
                    {
                        if (buff.atkBuffDict.ContainsKey(buffName))
                        {
                            buffName += "1";
                        }
                        else
                        {
                            buff.atkBuffDict.Add(buffName, buffAmount); //���� ����
                            StartCoroutine(BuffTime(buff, duration)); //���� ���ӽð� �ڷ�ƾ
                            break;
                        }
                    }
                }
                else // ���ӽð��� ���� ������ ���
                {
                        while (true)
                        {
                            if (buff.atkBuffDict.ContainsKey(buffName))
                            {
                                buffName += "1";
                            }
                            else
                            {
                                buff.atkBuffDict.Add(buffName, buffAmount); //���� ����
                                break;
                            }
                        }
                }
            }
            else // ��ø�� �Ұ����� ���
            {
                if (hasDuration) // ���ӽð��� �ִ� ������ ���
                {
                    buff.atkBuffDict.Remove(buffName);
                    buff.atkBuffDict.Add(buffName, buffAmount); //���� �߰�
                   StartCoroutine(BuffTime(buff, duration)); //���� ���ӽð� �ڷ�ƾ

                }
                else // ���ӽð��� ���� ������ ���
                {
                    buff.atkBuffDict.Remove(buffName);
                    buff.atkBuffDict.Add(buffName, buffAmount); //���� �߰�
                }
            }
            
            //buff.atkBuffDict.Add(buffName, buffAmount); 
            buffable.getBuff = buff; // ���� ����
            Debug.Log("������" + buffName);
        }
        */
}

    protected override void RemoveBuff(Collider other)
    {
        IBuffable buffable = other.GetComponent<IBuffable>();
        if (buffable != null)
        {
            Buff buff = buffable.getBuff; // ������ ���� ��������
            if (buff == null)
            {
                buff = new Buff(); // ������ ������ ���� ����
            }
            switch (BData.buffType)
            {
                case BuffBuildingData.BuffType.Heal:
                    // ó���� ����
                    break;
                case BuffBuildingData.BuffType.atkBuff:
                    buffType = buff.atkBuffDict;
                    break;
                case BuffBuildingData.BuffType.hpBuff:
                    buffType = buff.hpBuffDict;
                    break;
                case BuffBuildingData.BuffType.asBuff:
                    buffType = buff.asBuffDict;
                    break;
                case BuffBuildingData.BuffType.msBuff:
                    buffType = buff.msBuffDict;
                    break;
                case BuffBuildingData.BuffType.rangeBuff:
                    buffType = buff.rangeBuffDict;
                    break;
                default:
                    break;
            }
            buffController.RemoveBuff(buffable, buffType);
        }
        /*
        //targets ����Ʈ�� �ش� ������Ʈ�� �ִ��� üũ
        IBuffable buffable = other.GetComponent<IBuffable>();
        Debug.Log(buffable);
        if (buffable != null)
        {
            Buff buff = buffable.getBuff; // ������ ���� ��������
            if (buff == null)
            {
                //buff = new Buff(); // ������ ������ ���� ����
                return;
            }
            // ���� �� ����
            //buff.atkBuffList.Add(buffAmount);
            buff.atkBuffDict.Remove(buffName); // 0419 ������

            buffable.getBuff = buff; // ���� ����
            Debug.Log("����������");
        }
        */

    }
    /*
    IEnumerator BuffTime(Buff buff, float dur)
    {
        yield return new WaitForSeconds(dur);
        buff.atkBuffDict.Remove(buffName); // ���ӽð��� ������ ���� ����
    }
    */

    protected override void Destroy() //�ı��� ȣ��Ǵ� Destroy�Լ�. �ı����� �ְ��ִ� ������ ��� �����Ѵ�
    {
        if (targets.Count > 0)
        {
            foreach (GameObject target in targets) //targets list�� <GameObject>�� �Ǿ��ִ�. ���� Collider�� ���� ������ ��� ������ ��.
            {
                IBuffable buffable = target.GetComponent<IBuffable>();
                if (buffable != null)
                {
                    Buff buff = buffable.getBuff; // ������ ���� ��������
                    if (buff == null)
                    {
                        //buff = new Buff(); // ������ ������ ���� ����
                        return;
                    }
                    // ���� �� ����
                    //buff.atkBuffList.Add(buffAmount);
                    buff.atkBuffDict.Remove(buffController.BuffName);

                    
                    buffable.getBuff = buff; // ���� ����
                }
            }
            
        }
        base.Destroy();
    }

}
