using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 몬스터상호작용
/// 
/// </summary>

public interface IDamage
{
    void TakeDamage(short dmg);   
}
public class MonsterAction : MonoBehaviour
{
    Monster myMonster;
    [SerializeField] LayerMask Building;
    [SerializeField] LayerMask Player;

    private void Start()
    {
        Building = LayerMask.GetMask("Building");
        Player = LayerMask.GetMask("Player");
        TryGetComponent(out myMonster);
    }

    //테스트용코드
    private void OnCollisionEnter(Collision collision)
    {
        if (!myMonster.isAttack)
        {
            if ((Building & 1 << collision.gameObject.layer) != 0)
            {
                myMonster.isBlocked = true;
                StartCoroutine(TakeDamaging(collision.gameObject.GetComponent<IDamage>()));
                Debug.Log("건물 충돌");
            }
            else if ((Player & 1 << collision.gameObject.layer) != 0)
            {
                Debug.Log("플레이어 충돌");
                //StartCoroutine(TakeDamaging(collision.gameObject.GetComponent<IDamage>()));
            }
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (!myMonster.isAttack)
        {
            if ((Building & 1 << collision.gameObject.layer) != 0)
            {
                StartCoroutine(TakeDamaging(collision.gameObject.GetComponent<IDamage>()));
                Debug.Log("건물 충돌");
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
