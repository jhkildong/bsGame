using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public short Attack;

    public void onDamage(IDamage obj)
    {
        obj.TakeDamage(Attack);
    }
}
