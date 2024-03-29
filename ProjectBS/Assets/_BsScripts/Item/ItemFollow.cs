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

    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (target != null)
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
    }
    private void OnTriggerEnter(Collider other)
    {
        SphereCollider sphereCollider = other.GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            willDie = other.GetComponent<CapsuleCollider>().radius;
            movespeed = other.GetComponent<wasdMoving>().moveSpeed + other.GetComponent<SphereCollider>().radius;
            target = other.transform;
            Debug.Log("Item is following..");
        }
    }
    void Eat()
    {
        Destroy(gameObject);
        Debug.Log(gameObject + "을(를) 얻었다..!");
    }
}
