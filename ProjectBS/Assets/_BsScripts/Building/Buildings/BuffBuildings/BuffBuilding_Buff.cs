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

    [SerializeField] private float buffAmount; // ������

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
            // ���� �� ����
            buff.atkBuff = 1.5f; // ���÷� ���ݷ� ������ 10���� ����
            buffable.getBuff = buff;
            
        }
    }
}
