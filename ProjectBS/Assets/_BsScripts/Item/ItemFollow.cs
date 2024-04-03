using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollow : MonoBehaviour
{
    Transform target;
    Vector3 dir;
    float movespeed;
    float accel;
    public void follow(PlayerA playerA)
    {
        Debug.Log("Item is following...");
        StartCoroutine(following(playerA));
    }
    IEnumerator following(PlayerA playerA)
    {
        target = playerA.transform;
        while(target != null)
        {
            dir = playerA.transform.position - transform.position;
            accel += Time.deltaTime;
            transform.position += dir.normalized * accel;
            if (Vector3.Distance(target.position, transform.position) < 1f)
            {
                Eat();  
                yield break;
            }
            yield return null;
        }
        
        /*while (target != null)
        {
            dir = playerA.transform.position - transform.position;
            transform.position += dir.normalized * 0.01f * Time.deltaTime;
            Debug.Log(transform.position);
            if (Vector3.Distance(target.position, transform.position) < 0.1f)
            {
                //Eat();
                target = null;
            }
        }*/
    }
    void Eat()
    {
        Destroy(gameObject);
        Debug.Log(gameObject + "을(를) 얻었다..!");
    }
}
