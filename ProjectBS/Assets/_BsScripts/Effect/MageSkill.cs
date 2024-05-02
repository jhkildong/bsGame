using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MageSkill : PlayerSkill
{
    private float attackInterval = 0.3f;
    Coroutine takingDamage;

    private void OnEnable()
    {
        takingDamage = StartCoroutine(TakingDamage());
    }

    private void OnDisable()
    {
        StopCoroutine(takingDamage);
    }


    IEnumerator TakingDamage()
    {
        while(true)
        {
            yield return new WaitForSeconds(attackInterval);
            TakeDamage();
        }
    }

    void TakeDamage()
    {
        List<Collider> colliders = new();
        colliders.AddRange(Physics.OverlapSphere(transform.position + transform.forward * 5.0f, 3.0f, (int)BSLayerMasks.Monster | (int)BSLayerMasks.SurroundMonster));
        colliders.AddRange(Physics.OverlapSphere(transform.position + transform.forward * 1.5f, 1.5f, (int)BSLayerMasks.Monster | (int)BSLayerMasks.SurroundMonster));

        foreach (var collider in colliders)
        {
            IDamage damage = collider.GetComponent<IDamage>();
            if (damage != null)
            {
                damage.TakeDamage(Attack);
            }
        }
    }

}
