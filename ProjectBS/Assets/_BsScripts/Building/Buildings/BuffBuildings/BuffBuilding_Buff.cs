using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBuilding_Buff : BuffBuildingBase
{

    [SerializeField] private BuffBuildingData BuffBuildingData;
    public BuffBuildingData BData
    {
        get { return BuffBuildingData; }
        set { BuffBuildingData = value; }
    }

    [SerializeField] private float buffAmount; // 버프량

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        targetLayer = BData.targetLayer;
        buffAmount = BData.buffAmount;
    }

    protected override void StartBuff(Collider other)
    {
        IBuffable buffable = other.GetComponent<IBuffable>();
        if (buffable != null)
        {
            Buff buff = new Buff();
            // 버프 값 설정
            buff.atkBuff = 1.5f; // 예시로 공격력 버프를 10으로 설정
            buffable.getBuff = buff;
            
        }
    }
}
