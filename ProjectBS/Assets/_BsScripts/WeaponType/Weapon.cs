using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour, IPoolable
{
    protected LayerMask Monster;
    public float Ak;

    protected virtual void Start()
    {
        Monster = (int)BSLayerMasks.Monster | (int)BSLayerMasks.SurroundMonster;
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((Monster & 1 << other.gameObject.layer) != 0)
        {
            IDamage<Monster> obj = other.GetComponent<IDamage<Monster>>();
            if (obj != null)
            {
                obj.TakeDamageEffect(Ak);
            }
        }
    }
    public void SetID(int id)
    {
        _id = id;
    }

    public int ID => _id;
    private int _id;
    public MonoBehaviour This => this;
    public IPoolable CreateClone()
    {
        Weapon clone = Instantiate(this);
        clone.SetID(ID);
        return clone;
    }
}
