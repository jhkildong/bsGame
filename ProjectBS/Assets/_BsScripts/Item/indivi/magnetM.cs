using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnetM : MonoBehaviour
{
    Transform target;
    Vector3 dir;
    float movespeed;
    float elapseTime;
    float accle = 2f;
    float willDie;
    // Start is called before the first frame update
    void Start()
    {

    }

    void Update()
    {
        if (target != null)
        {
            float currentSpeed = movespeed + accle * elapseTime;
            elapseTime += Time.deltaTime;
            dir = target.position - transform.position;
            transform.position += dir.normalized * movespeed * Time.deltaTime;
            //Debug.Log(Time.deltaTime);
            //movespeed += 1f;
            //transform.position = target.position;
            if (Vector3.Distance(target.position, transform.position) < willDie)
            {
                Eat();
                target = null;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        willDie = other.GetComponent<CapsuleCollider>().radius;
        movespeed = other.GetComponent<wasdMoving>().moveSpeed + other.GetComponent<SphereCollider>().radius;
        target = other.transform;
        //transform.position = target.position;
    }
    void Eat()
    {
        Destroy(gameObject);
        Debug.Log(gameObject);
    }
}
