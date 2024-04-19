using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : MonoBehaviour
{
    Transform target;
    Vector3 dir;
    //public float AttackRange = 3;
    // Start is called before the first frame update
    public Vector3 A;
    void Start()
    {
        target = GameObject.Find("Player").transform;
        Debug.Log(target);
    }

    // Update is called once per frame
    void Update()
    {
        A = target.position - gameObject.transform.position;
        if (A.magnitude > 3)
        {
            if (target != null)
            {
                dir = target.position - transform.position;
                transform.position += dir.normalized * Time.deltaTime;
            }
        }
        else if (A.magnitude < 3)
        {
            gameObject.transform.position += Vector3.up;
        }
    }

    IEnumerable SplashDamage()
    {
        if (target != null)
        {
            dir = target.position - transform.position;
            transform.position += dir.normalized * Time.deltaTime;
        }
        yield return null;
    }
}
