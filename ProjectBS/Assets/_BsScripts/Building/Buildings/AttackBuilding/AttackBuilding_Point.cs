using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBuilding_Point : AttackBuildingBase
{
    protected override void Start()
    {
        base.Start();
        AtkEvent.AddListener(PointAttack);
        Debug.Log("attackbuilding_Point" + _constTime);
    }

    protected void PointAttack()
    {

        SetActivePointAtkEffect();

    }

    void SetActivePointAtkEffect()
    {
        for (int i = 0; i < effectList.Count; i++)
        {

            if (effectList[i].activeSelf == false)
            {
                effectList[i].transform.position = target.transform.position + new Vector3(0, 0.2f, 0);
                effectList[i].SetActive(true);
                break;
            }


            else
            {
                if (i == effectList.Count - 1)
                {
                    InstEffects();
                    Debug.Log("QDSFSADF");
                }
                continue;
            }
        }
    }


}
