using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitEffects : MonoBehaviour
{
    ParticleSystem ps;
    public int ID => id;
    [SerializeField] private int id;
    float progress; // ��ƼŬ ��� ���൵

    void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        progress = ps.time / ps.main.duration;
        //if (Mathf.Approximately(progress, hitTime) && canHit)

        if (progress >= 1f) // ����Ʈ�� ������
        {
            //EffectPoolManager.Instance.ReleaseObject<NonAtkEffect>(gameObject); //Ǯ�� �ǵ���
            EffectPoolManager.Instance.ReleaseObject(gameObject,id); //Ǯ�� �ǵ���
        }

    }
}
