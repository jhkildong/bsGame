using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollow : MonoBehaviour
{
    Transform target;
    Vector3 dir;
    float moveSpeed = 0.5f;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        /*BoxCollider boxCollider = GetComponent<BoxCollider>();
        Vector3 size = boxCollider.size;*/
    }

    // Update is called once per frame
    void Update()
    {
        dir = target.position - transform.position;
        if (dir.magnitude > 1)
        {
            dir.Normalize();
        }
       
    }
    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // �浹�� Ÿ�� �������� �������� �̵���ŵ�ϴ�.
            transform.position += dir * moveSpeed * Time.deltaTime;
        }
    }
}
