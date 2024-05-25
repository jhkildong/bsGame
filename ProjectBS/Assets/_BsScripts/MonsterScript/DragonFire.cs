using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonFire : MonoBehaviour
{
    private float attackInterval = 0.3f;
    Coroutine takingDamage;
    public float Attack { get; set; }

    [SerializeField] private HitEffects hitEffectPrefab; //박지민 추가 (타격 이펙트)
    public void StartEffect()
    {
        takingDamage = StartCoroutine(TakingDamage());
    }

    public void StopEffect()
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
        colliders.AddRange(Physics.OverlapSphere(transform.position + transform.forward * 2.2f, 1.0f, (int)BSLayerMasks.Player));
        colliders.AddRange(Physics.OverlapSphere(transform.position + transform.forward * 0.7f, 0.5f, (int)BSLayerMasks.Player));

        foreach (var collider in colliders)
        {
            IDamage damage = collider.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.TakeDamageEffect(Attack);
                EffectPoolManager.Instance.SetActiveHitEffect(hitEffectPrefab, collider.transform.position, hitEffectPrefab.ID); //피격이펙트 생성
            }
        }
    }
}
