using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBuilding_Heal : BuffBuildingBase
{

    [SerializeField] private HealingBuildingData HealingBuildingData;
    public HealingBuildingData HData
    {
        get { return HealingBuildingData; }
        set { HealingBuildingData = value; }
    }
    [SerializeField] private HitEffects healEffect;
    [SerializeField] private float healAmount; // 힐량
    [SerializeField] private float healDelay; // 힐 틱당 딜레이
    [SerializeField] private bool hasDuratuon; // 지속시간이 있는가?
    [SerializeField] private float duration; // 건물 지속시간
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        targetLayer = HData.targetLayer;
        healAmount = HData.healAmount;
        healDelay = HData.healDelay;
        hasDuratuon = HData.hasDuration;
        duration = HData.duration;
    }


    protected override void StartBuff(Collider other)
    {
        StartCoroutine(healTickTimeCheck(other.GetComponent<IHealing>(), other.gameObject));
    }
    protected override void ConstructComplete() //건설 완료시
    {
        base.ConstructComplete();
        if(hasDuratuon)
        StartCoroutine(lifeSpan());// 지속시간 설정
    }
    IEnumerator lifeSpan()
    {
        while (duration >= 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Campfire 지속시간 끝");
        Destroy();
        
    }

    IEnumerator healTickTimeCheck(IHealing healable, GameObject obj)
    {
        if (obj == null) yield break;
        float inTime = 0;
        while (obj != null && targets.Contains(obj))
        {
            if (inTime < healDelay)
            {
                inTime += Time.deltaTime;
            }
            else if (inTime >= healDelay)
            {
                Debug.Log("힐!");
                if (targets.Contains(obj))
                {
                    healable.ReceiveHealEffect(healAmount);
                    EffectPoolManager.Instance.SetParentEffect(healEffect, healEffect.ID, obj.transform); // 힐 이펙트를 받는 객체 자식으로 생성밑 풀링
                }
                else
                {

                }
                //activeHeal();
                inTime = 0;
            }
            yield return null;
        }
        if (targets.Contains(obj)) targets.Remove(obj);
        yield return null;
    }
}
