using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_Script : BossMonster
{
    Transform target;
    private Animator animator;
    float Hp;
    void Start()
    {
        animator = GetComponent<Animator>();
        Hp = 100;
        ChangeHpAct += ChangeHp;
    }
    private void OnTriggerEnter(Collider other)
    {
        Hp -= 20;
        Debug.Log("현재 체력 : " + Hp);
        if(16 == other.gameObject.layer)
        {
            animator.SetFloat("groundLocomotion", 0.5f);
            target = other.gameObject.transform;
            //animator.SetTrigger("goAir");

            /*while(transform.position.y < 4) {
                transform.position += Vector3.up;
            }*/
        }
    }

    public void ChangeHp(float Hp)
    {
        if (Hp > 0.6)
        {

        }
        else if (Hp > 0.6)
        {

        }
        else if (Hp < 0.6)
        {

        }
        else
        {
            animator.SetTrigger("Dead");
        }
    }
}