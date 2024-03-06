using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAction : MonoBehaviour
{
    Monster myMonster;

    private void Start()
    {
        myMonster = GetComponent<Monster>();
    }

    //테스트용코드
    private void OnCollisionEnter(Collision collision)
    {
        if(GetComponent<MonsterFollowPlayer>().target == collision.transform)
        {
            myMonster.TakeDamage(myMonster.MaxHP);
        }
    }

}
