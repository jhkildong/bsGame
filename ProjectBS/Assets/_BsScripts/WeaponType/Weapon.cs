using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    private LayerMask Monster;
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
                obj.TakeDamage(Ak);
            }
        }
    }
}
