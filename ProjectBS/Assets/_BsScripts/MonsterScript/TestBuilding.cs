using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBuilding : MonoBehaviour
{
    public short HP = 1000;

    public void TakeDamage(short dmg)
    {

        HP -= dmg;
    }
}
