using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonAtkEffect : MonoBehaviour
{
    ParticleSystem ps;
    protected float hitTime = 1f; //���� Ÿ�̹�
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
            EffectPoolManager.Instance.ReleaseObject<NonAtkEffect>(gameObject); //Ǯ�� �ǵ���
        }

    }
}
