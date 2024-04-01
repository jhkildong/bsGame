using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Effect : MonoBehaviour, IPoolable<Effect>
{
    public int ID => _id;
    
    public float Attack { get => _attack; set => _attack = value; }
    public float Speed { get => _speed; set => _speed = value; }
    public float Size { get => _size; set => _size = value; }

    [SerializeField] private int _id;
    [SerializeField] private float _attack;
    [SerializeField] private float _speed;
    [SerializeField] private float _size;
    private ParticleSystem[] _particleSystem;
    public GameObject prefab;

    public GameObject CreateClone()
    {
        GameObject clone = Instantiate(prefab);
        _particleSystem = clone.GetComponentsInChildren<ParticleSystem>();

        return clone;
    }
    private void OnEnable()
    {
        foreach(var particle in _particleSystem)
        {
            particle.Play();
        }
    }

    void Update()
    {
    }


    private void OnParticleCollision(GameObject other)
    {
        if (other.TryGetComponent(out IDamage<Monster> monster))
        {
            monster.TakeDamage((short)Attack);
        }
    }
}