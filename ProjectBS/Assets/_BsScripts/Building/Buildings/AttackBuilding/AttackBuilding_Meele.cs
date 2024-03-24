using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class AttackBuilding_Meele : AttackBuildingBase
{
    protected override void Start()
    {
        base.Start();
        AtkEvent.AddListener(MeeleAttack);
        Debug.Log("attackbuilding_Meele" + _constTime);
    }

    protected void MeeleAttack()
    {
        SetActiveMeeleAtkEffect();
        MeeleAtk();
    }

    void SetActiveMeeleAtkEffect()
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
                }
                continue;
            }
        }
    }

    void MeeleAtk()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, new Vector3(2,2,2));
        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                IDamage target = collider.GetComponent<IDamage>();
                target.TakeDamage(_attackPower);
                Debug.Log("근접 건물 공격!");
            }
        }
    }
}
