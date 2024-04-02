using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollow : MonoBehaviour
{
    Transform target;
    Vector3 dir;
    float movespeed;
    float elapseTime;
    float accle = 2f;
    float willDie;
    int countCollider = 0;

    void Update()
    {
    }
    private void OnTriggerEnter(Collider other)
    {
        if ((int)(BSLayerMasks.MagneticField) == (1 << other.gameObject.layer))
        {
            willDie = other.GetComponentInParent<CapsuleCollider>().radius;
            movespeed = other.GetComponentInParent<wasdMoving>().moveSpeed + other.GetComponentInParent<SphereCollider>().radius;
            target = other.transform;
        }
    }

    public void test(PlayerA playerA)
    {
        Debug.Log("recieved");
        StartCoroutine(follow());
    }
    IEnumerator follow()
    {
        Debug.Log("Item is following..");
        while (target != null)
        {
            float currentSpeed = movespeed + accle * elapseTime;
            elapseTime += Time.deltaTime;
            dir = target.position - transform.position;
            transform.position += dir.normalized * movespeed * Time.deltaTime;
            if (Vector3.Distance(target.position, transform.position) < willDie)
            {
                Eat();
                target = null;
            }
        }
        yield return null;
    }
    void Eat()
    {
        Destroy(gameObject);
        Debug.Log(gameObject + "을(를) 얻었다..!");
    }
}
