using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponLP_Bullet : Weapon
{
    float DelayTime = 0.2f;
    float time;

    [SerializeField] private HitEffects hitEffectPrefab; //박지민 추가 (타격 이펙트)
    private void OnEnable()
    {
        StartCoroutine(DelayRelease(5.0f));
    }

    IEnumerator DelayRelease(float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPoolManager.Instance.ReleaseObj(this);
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        if (time >= DelayTime)
        {
            Collider[] list = Physics.OverlapSphere(transform.position, 2.0f, Monster);
            time = 0.0f;
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
        }
    }
}
