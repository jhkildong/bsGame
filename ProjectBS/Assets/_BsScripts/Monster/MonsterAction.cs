using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterAction : MonoBehaviour
{
    Monster myMonster;
    [SerializeField] LayerMask Building;
    [SerializeField] LayerMask Player;

    
    private void Start()
    {
        Building = LayerMask.NameToLayer("Building");
        Player = LayerMask.NameToLayer("Player");
        TryGetComponent(out myMonster);
    }

    //테스트용코드
    private void OnCollisionEnter(Collision collision)
    {
        if (myMonster == null) return;
        if (!myMonster.isAttack)
        {
            if ((Building & 1 << collision.gameObject.layer) != 0)
            {
                StartCoroutine(TakeDamaging(collision.gameObject.GetComponent<IDamage>()));
            }
            else if ((Player & 1 << collision.gameObject.layer) != 0)
            {
                //myMonster.TakeDamage(myMonster.Data.MaxHP);
                StartCoroutine(TakeDamaging(collision.gameObject.GetComponent<IDamage>()));
            }
        }
    }

    IEnumerator TakeDamaging(IDamage obj)
    {
        obj.TakeDamage((short)myMonster.Attack);
        while(myMonster.CurAttackDelay > 0)
        {
            myMonster.CurAttackDelay -= Time.deltaTime;
            yield return null;
        }
        myMonster.ResetAttackDelay();
    }

}
