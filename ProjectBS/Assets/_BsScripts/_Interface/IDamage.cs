using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void TakeDamage(float dmg);
}

public interface IDamage<T> : IDamage where T : MonoBehaviour
{

}
