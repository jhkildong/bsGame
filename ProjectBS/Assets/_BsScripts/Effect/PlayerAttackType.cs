using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackType : MonoBehaviour, IPoolable
{
    #region IPoolable
    public MonoBehaviour This => this;
    public int ID => _id;

    public IPoolable CreateClone()
    {
        GameObject clone = Instantiate(prefab);

        return clone.GetComponent<PlayerAttackType>();
    }
    #endregion

    #region Property
    public string Name { get => _name;}
    public float Attack { get => _attack; set => _attack = value; }
    public float Size
    { 
        get => _size;
        set
        {
            if(value != _size)
            {
                _size = value;
                transform.localScale = Vector3.one * _size;
            }
        }
    }
    #endregion

    #region Field
    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private float _attack;
    [SerializeField] private float _size;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject prefab;
    [SerializeField] private GameObject hitEffectPrefab;
    #endregion

    protected bool isStopped = false;

    protected virtual void Awake()
    {
        this.gameObject.layer = (int)BSLayers.PlayerAttackEffect;
    }

    protected virtual void OnEnable()
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
            if(hitEffectPrefab != null)
            EffectPoolManager.Instance.SetActiveEffect<GameObject>(hitEffectPrefab, other.gameObject); //피격이펙트 생성
        }
    }

}