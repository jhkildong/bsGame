using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamage
{
    void TakeDamage(short dmg);
}

public interface IDamage<T> where T : MonoBehaviour
{
    void TakeDamage(short dmg);
}
