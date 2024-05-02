using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Effect : MonoBehaviour, IPoolable
{
    #region IPoolable
    public MonoBehaviour This => this;
    public int ID => _id;

    public IPoolable CreateClone()
    {
        GameObject clone = Instantiate(prefab);

        return clone.GetComponent<Effect>();
    }
    #endregion

    #region Property
    public string Name { get => _name;}
    public float Attack { get => _attack; set => _attack = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float Size { get => _size; set => _size = value; }
    #endregion

    #region Field
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private float _attack;
    [SerializeField] private float _speed;
    [SerializeField] private float _size;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject prefab;
    [SerializeField] private HitEffects hitEffectPrefab;
    #endregion

    protected bool isStopped = false;

    protected virtual void Awake()
    {
        this.gameObject.layer = (int)BSLayers.PlayerAttackEffect;
    }

    private void OnEnable()
    {
        if(_particleSystem == null)
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }
        _particleSystem.Play();
        StartCoroutine(EffectPlaying());
    }

    private IEnumerator EffectPlaying()
    {
        while (true)
        {
            if (isStopped || _particleSystem.isStopped)
            {
                isStopped = false;
                ObjectPoolManager.Instance.ReleaseObj(this);   //onDisable
                yield break;
            }
            yield return null;
        }
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamage<Monster> monster))
        {
            //monster.TakeDamage(Attack);
            monster.TakeDamageEffect(Attack);
            Vector3 contact = other.ClosestPoint(transform.position); // 충돌한 위치와 가장 가까운 점을 찾는다.
            Debug.Log(contact);
            if (hitEffectPrefab != null)

            EffectPoolManager.Instance.SetActiveHitEffect(hitEffectPrefab, contact, hitEffectPrefab.ID); //피격이펙트 생성
        }
    }

}