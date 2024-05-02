using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffects : MonoBehaviour
{
    ParticleSystem ps;
    public int ID => id;
    [SerializeField] private int id;
    float progress; // 파티클 재생 진행도

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        progress = ps.time / ps.main.duration;
        //if (Mathf.Approximately(progress, hitTime) && canHit)

        if (progress >= 1f) // 이펙트가 끝나면
        {
            //EffectPoolManager.Instance.ReleaseObject<NonAtkEffect>(gameObject); //풀로 되돌림
            EffectPoolManager.Instance.ReleaseObject(gameObject,id); //풀로 되돌림
        }

    }
}
