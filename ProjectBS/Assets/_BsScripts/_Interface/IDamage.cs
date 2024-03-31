using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void TakeDamage(short dmg);
}

public interface IDamage<T> : IDamage where T : MonoBehaviour
{

}
