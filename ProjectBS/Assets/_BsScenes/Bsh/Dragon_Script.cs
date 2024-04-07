using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragon_Script : MonoBehaviour
{
    Transform target;
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetTrigger("fireBreath");
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(16 == other.gameObject.layer)
        {
            animator.SetFloat("groundLocomotion", 0.2f);
            target = other.gameObject.transform;
            //animator.SetTrigger("goAir");

            while(transform.position.y < 4) {
                transform.position += Vector3.up;
            }
        }
    }
}