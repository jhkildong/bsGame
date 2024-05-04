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

    BuffDict buffType = new BuffDict();
    //Dictionary<string, float> buffType = null;
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

    
    
    protected override void StartBuff(Collider other) // ontriggerenter시
    {
        IBuffable buffable = other.GetComponent<IBuffable>();
        if (buffable != null)
        {
            Buff buff = buffable.getBuff; // 기존의 버프 가져오기
            if (buff == null)
            {
                buff = new Buff(); // 버프가 없으면 새로 생성
            }
            switch (BData.buffType)
            {
                case BuffBuildingData.BuffType.Heal:
                    // 처리할 내용
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
            Buff buff = buffable.getBuff; // 기존의 버프 가져오기
            if (buff == null)
            {
                buff = new Buff(); // 버프가 없으면 새로 생성
            }
            // 버프 값 설정
            //buff.atkBuffList.Add(buffAmount);
            if(canStack) // 중첩이 가능한 경우
            {
                if (hasDuration) // 지속시간이 있는 버프의 경우
                {
                    while (true)
                    {
                        if (buff.atkBuffDict.ContainsKey(buffName))
                        {
                            buffName += "1";
                        }
                        else
                        {
                            buff.atkBuffDict.Add(buffName, buffAmount); //버프 갱신
                            StartCoroutine(BuffTime(buff, duration)); //버프 지속시간 코루틴
                            break;
                        }
                    }
                }
                else // 지속시간이 없는 버프의 경우
                {
                        while (true)
                        {
                            if (buff.atkBuffDict.ContainsKey(buffName))
                            {
                                buffName += "1";
                            }
                            else
                            {
                                buff.atkBuffDict.Add(buffName, buffAmount); //버프 갱신
                                break;
                            }
                        }
                }
            }
            else // 중첩이 불가능한 경우
            {
                if (hasDuration) // 지속시간이 있는 버프의 경우
                {
                    buff.atkBuffDict.Remove(buffName);
                    buff.atkBuffDict.Add(buffName, buffAmount); //버프 추가
                   StartCoroutine(BuffTime(buff, duration)); //버프 지속시간 코루틴

                }
                else // 지속시간이 없는 버프의 경우
                {
                    buff.atkBuffDict.Remove(buffName);
                    buff.atkBuffDict.Add(buffName, buffAmount); //버프 추가
                }
            }
            
            //buff.atkBuffDict.Add(buffName, buffAmount); 
            buffable.getBuff = buff; // 버프 적용
            Debug.Log("버프됨" + buffName);
        }
        */
}

    protected override void RemoveBuff(Collider other)
    {
        IBuffable buffable = other.GetComponent<IBuffable>();
        if (buffable != null)
        {
            Buff buff = buffable.getBuff; // 기존의 버프 가져오기
            if (buff == null)
            {
                buff = new Buff(); // 버프가 없으면 새로 생성
            }
            switch (BData.buffType)
            {
                case BuffBuildingData.BuffType.Heal:
                    // 처리할 내용
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
        //targets 리스트에 해당 오브젝트가 있는지 체크
        IBuffable buffable = other.GetComponent<IBuffable>();
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
            buff.atkBuffDict.Remove(buffName); // 0419 수정중

            buffable.getBuff = buff; // 버프 적용
            Debug.Log("버프해제됨");
        }
        */

    }
    /*
    IEnumerator BuffTime(Buff buff, float dur)
    {
        yield return new WaitForSeconds(dur);
        buff.atkBuffDict.Remove(buffName); // 지속시간이 끝나면 버프 제거
    }
    */

    public override void Destroy() //파괴시 호출되는 Destroy함수. 파괴전에 주고있는 버프를 모두 제거한다
    {
        if (targets.Count > 0)
        {
            foreach (GameObject target in targets) //targets list가 <GameObject>로 되어있다. 추후 Collider로 수정 가능할 경우 수정할 것.
            {
                if(target == null) //타겟이 파괴되었을 경우 다음 타겟으로 넘어간다
                {
                    continue;
                }
                IBuffable buffable = target.GetComponent<IBuffable>();
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
                    //buff.atkBuffDict.Remove(buffController.BuffName);
                    buffController.RemoveBuff(buffable, buffType); // 버프 제거
                    buffable.getBuff = buff; //버프제거후 적용
                }
            }
            
        }
        base.Destroy();
    }

}
