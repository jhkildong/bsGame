using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuffController
{
    BuffController buffController { get; set; }
}

public class BuffController
{
    public MonoBehaviour This;
    public string BuffName;
    public float BuffAmount;
    public bool HasDuration;
    public float Duration;
    public bool CanStack;

    public BuffController(MonoBehaviour mono, string name, float amount, bool hasDuration, float dur, bool stack)
    {
        This = mono;
        BuffName = name;
        BuffAmount = amount;
        HasDuration = hasDuration;
        Duration = dur;
        CanStack = stack;
    }


    public void StartBuff(IBuffable buffable) // ontriggerenter��
    {
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
            if (CanStack) // ��ø�� ������ ���
            {
                if (HasDuration) // ���ӽð��� �ִ� ������ ���
                {
                    while (true)
                    {
                        if (buff.atkBuffDict.ContainsKey(BuffName))
                        {
                            BuffName += "1";
                        }
                        else
                        {
                            buff.atkBuffDict.Add(BuffName, BuffAmount); //���� ����
                            This.StartCoroutine(BuffTime(buff, Duration)); //���� ���ӽð� �ڷ�ƾ
                            break;
                        }
                    }
                }
                else // ���ӽð��� ���� ������ ���
                {
                    while (true)
                    {
                        if (buff.atkBuffDict.ContainsKey(BuffName))
                        {
                            BuffName += "1";
                        }
                        else
                        {
                            buff.atkBuffDict.Add(BuffName, BuffAmount); //���� ����
                            break;
                        }
                    }
                }
            }
            else // ��ø�� �Ұ����� ���
            {
                if (HasDuration) // ���ӽð��� �ִ� ������ ���
                {
                    buff.atkBuffDict.Remove(BuffName);
                    buff.atkBuffDict.Add(BuffName, BuffAmount); //���� �߰�
                    This.StartCoroutine(BuffTime(buff, Duration)); //���� ���ӽð� �ڷ�ƾ

                }
                else // ���ӽð��� ���� ������ ���
                {
                    buff.atkBuffDict.Remove(BuffName);
                    buff.atkBuffDict.Add(BuffName, BuffAmount); //���� �߰�
                }
            }

            //buff.atkBuffDict.Add(buffName, buffAmount); 
            buffable.getBuff = buff; // ���� ����
            Debug.Log("������" + BuffName);
        }
    }

    public void RemoveBuff(IBuffable buffable)
    {
        //targets ����Ʈ�� �ش� ������Ʈ�� �ִ��� üũ
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
            buff.atkBuffDict.Remove(BuffName); // 0419 ������

            buffable.getBuff = buff; // ���� ����
            Debug.Log("����������");
        }
    }

    IEnumerator BuffTime(Buff buff, float dur)
    {
        yield return new WaitForSeconds(dur);
        buff.atkBuffDict.Remove(BuffName); // ���ӽð��� ������ ���� ����
    }
}