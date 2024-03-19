using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBuilding : AttackBuilding
{
    public GameObject atkEffect;
    public List<GameObject> effectList;
    public Transform effectPool;

    protected override void Start()
    {
        base.Start();
        for(int i = 0; i < 5; i++)
        {
            effectList.Add(atkEffect); //이펙트를 리스트에 다수 생성 (임시)
        }
    }
    protected override void RangeAttack()
    {
        SetActiveAtkEffect();
    }

    void SetActiveAtkEffect()
    {
        for(int i = 0;i < effectList.Count;i++)
        {
            if (effectList[i].activeSelf == false)
            {
                effectList[i].SetActive(true);

                break;
            }
            else
            {
                continue;
            }
        }
    }

}
