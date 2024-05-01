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


    //public void StartBuff(IBuffable buffable, Dictionary<string,float> buffTypeDict) // ontriggerenter시
    public void StartBuff(IBuffable buffable, BuffDict buffTypeDict)
    {
        Debug.Log(buffable);
        if (buffable != null)
        {
            Buff buff = buffable.getBuff; // 기존의 버프 가져오기
            if (buff == null)
            {
                buff = new Buff(); // 버프가 없으면 새로 생성
            }
            // 버프 값 설정
            //buff.atkBuffList.Add(buffAmount);
            if (CanStack) // 중첩이 가능한 경우
            {
                if (HasDuration) // 지속시간이 있는 버프의 경우
                {
                    while (true)
                    {
                        if (buffTypeDict.ContainsKey(BuffName)) // buff.atkBuffDict -> buffType
                        {
                            BuffName += "1";
                        }
                        else
                        {
                            buffTypeDict.Add(BuffName, BuffAmount); //버프 갱신
                            This.StartCoroutine(BuffTime(buffTypeDict, Duration)); //버프 지속시간 코루틴 // 수정전 (buff, duration)
                            break;
                        }
                    }
                }
                else // 지속시간이 없는 버프의 경우
                {
                    while (true)
                    {
                        if (buffTypeDict.ContainsKey(BuffName))
                        {
                            BuffName += "1";
                        }
                        else
                        {
                            buffTypeDict.Add(BuffName, BuffAmount); //버프 갱신
                            break;
                        }
                    }
                }
            }
            else // 중첩이 불가능한 경우
            {
                if (HasDuration) // 지속시간이 있는 버프의 경우
                {
                    buffTypeDict.Remove(BuffName);
                    buffTypeDict.Add(BuffName, BuffAmount); //버프 추가
                    This.StartCoroutine(BuffTime(buffTypeDict, Duration)); //버프 지속시간 코루틴

                }
                else // 지속시간이 없는 버프의 경우
                {
                    buffTypeDict.Remove(BuffName);
                    buffTypeDict.Add(BuffName, BuffAmount); //버프 추가
                }
            }

            //buff.atkBuffDict.Add(buffName, buffAmount); 
            buffable.getBuff = buff; // 버프 적용
            Debug.Log("버프됨" + BuffName);
        }
    }

    //public void RemoveBuff(IBuffable buffable, Dictionary<string,float> buffTypeDict)
    public void RemoveBuff(IBuffable buffable, BuffDict buffTypeDict)
    {
        //targets 리스트에 해당 오브젝트가 있는지 체크
        Debug.Log(buffable);
        if (buffable != null)
        {
            Buff buff = buffable.getBuff; // 기존의 버프 가져오기
            if (buff == null)
            {
                //buff = new Buff(); // 버프가 없으면 새로 생성
                return;
            }
            // 버프 값 설정
            //buff.atkBuffList.Add(buffAmount);
            buffTypeDict.Remove(BuffName); // buff.atkBuffDict -> buffTypeDict

            buffable.getBuff = buff; // 버프 적용
            Debug.Log("버프해제됨");
        }
    }



    //IEnumerator BuffTime(Dictionary<string, float> buffTypeDict, float dur)
    IEnumerator BuffTime(BuffDict buffTypeDict, float dur)
    {
        yield return new WaitForSeconds(dur);
        buffTypeDict.Remove(BuffName); // 지속시간이 끝나면 버프 제거
    }
}