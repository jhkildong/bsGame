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
    [SerializeField] private float healAmount; // ����
    [SerializeField] private float healDelay; // �� ƽ�� ������
    [SerializeField] private bool hasDuratuon; // ���ӽð��� �ִ°�?
    [SerializeField] private float duration; // �ǹ� ���ӽð�
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
    protected override void ConstructComplete() //�Ǽ� �Ϸ��
    {
        base.ConstructComplete();
        if(hasDuratuon)
        StartCoroutine(lifeSpan());// ���ӽð� ����
    }
    IEnumerator lifeSpan()
    {
        while (duration >= 0)
        {
            duration -= Time.deltaTime;
            yield return null;
        }
        Debug.Log("Campfire ���ӽð� ��");
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
                Debug.Log("��!");
                if (targets.Contains(obj))
                {
                    healable.ReceiveHealEffect(healAmount);
                    EffectPoolManager.Instance.SetParentEffect(healEffect, healEffect.ID, obj.transform); // �� ����Ʈ�� �޴� ��ü �ڽ����� ������ Ǯ��
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
