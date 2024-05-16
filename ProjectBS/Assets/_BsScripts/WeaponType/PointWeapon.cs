using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointWeapon : Weapon
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private HitEffects hitEffectPrefab; //박지민 추가 (타격 이펙트)
    private void OnEnable()
    {
        _particleSystem.Play();
        StartCoroutine(EffectPlaying());
    }

    private IEnumerator EffectPlaying()
    {
        yield return new WaitForEndOfFrame();

        Collider[] list = Physics.OverlapSphere(transform.position, 2.0f, Monster);

        foreach (Collider col in list)
        {
            IDamage<Monster> obj = col.GetComponent<IDamage<Monster>>();
            if (obj != null)
            {
                obj.TakeDamageEffect(Ak);
                Debug.Log("Attack");
                EffectPoolManager.Instance.SetActiveHitEffect(hitEffectPrefab, col.transform.position, hitEffectPrefab.ID); //박지민 추가 (타격 이펙트)
            }
        }

        while (true)
        {
            if (_particleSystem.isStopped)
            {
                ObjectPoolManager.Instance.ReleaseObj(this);   //onDisable
                yield break;
            }
            yield return null;
        }
    }
}
