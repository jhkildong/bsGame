using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Effect : MonoBehaviour, IPoolable
{
    public MonoBehaviour This => this;
    public int ID => _id;
    
    public float Attack { get => _attack; set => _attack = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float Size { get => _size; set => _size = value; }

    [SerializeField] private int _id;
    [SerializeField] private float _attack;
    [SerializeField] private float _speed;
    [SerializeField] private float _size;
    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private GameObject prefab;

    public IPoolable CreateClone()
    {
        GameObject clone = Instantiate(prefab);
        
        return clone.GetComponent<Effect>();
    }
    private void OnEnable()
    {
        if(_particleSystem == null)
        {
            _particleSystem = GetComponent<ParticleSystem>();
            return;
        }
        _particleSystem.Play();
    }

    void Update()
    {
        if(_particleSystem.isStopped)
        {
            ObjectPoolManager.Instance.ReleaseObj(this, this.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out IDamage<Monster> monster))
        {
            monster.TakeDamage(Attack);
        }
    }

}