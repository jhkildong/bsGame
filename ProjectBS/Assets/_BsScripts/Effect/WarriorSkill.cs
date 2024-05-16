using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarriorSkill : PlayerSkill
{
    private float attackInterval = 0.3f;
    Coroutine takingDamage;

    [SerializeField] private HitEffects hitEffectPrefab; //박지민 추가 (타격 이펙트)
    private void OnEnable()
    {
        takingDamage = StartCoroutine(TakingDamage());
        transform.localScale = Vector3.one * Size;
    }

    private void OnDisable()
    {
        StopCoroutine(takingDamage);
    }


    IEnumerator TakingDamage()
    {
        while (true)
        {
            yield return new WaitForSeconds(attackInterval);
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        List<Collider> colliders = new();
        colliders.AddRange(Physics.OverlapSphere(transform.position, 2.0f * Size, (int)BSLayerMasks.Monster | (int)BSLayerMasks.SurroundMonster));

        foreach (var collider in colliders)
        {
            IDamage damage = collider.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.TakeDamageEffect(Attack);

                EffectPoolManager.Instance.SetActiveHitEffect(hitEffectPrefab, collider.transform.position, hitEffectPrefab.ID); //박지민 추가 (타격 이펙트)
            }
        }
    }
}
