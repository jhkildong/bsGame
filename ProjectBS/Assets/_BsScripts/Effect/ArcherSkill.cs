using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherSkill : PlayerSkill
{
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private ParticleSystem subPs;
    [SerializeField] ArcherSkillParticleCollsion sub;
    public Archer myParent;

    public void Start()
    {
        if (_particleSystem == null)
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
    }
    private void OnEnable()
    {
        transform.localScale = Vector3.one * Size;
        sub.Attack = Attack;
        _particleSystem.Play();
        ParticleSystem.EmissionModule emission = subPs.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(80 * Size);
        StartCoroutine(EffectPlaying());
    }

    private IEnumerator EffectPlaying()
    {
        while (true)
        {
            if (_particleSystem.isStopped)
            {
                transform.gameObject.SetActive(false);
                myParent.skillEffectStack.Push(this);       //TODO: 서로 커플링, 퍼블릭변수 문제 해결해야함
                yield break;
            }
            yield return null;
        }
    }
}
