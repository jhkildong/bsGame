using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollow : MonoBehaviour
{
    Transform target;
    Vector3 dir;
    float moveSpeed;
    bool follow = false;
    float sphereRange;
    bool flag;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        sphereRange = GameObject.Find("Player").GetComponent<SphereCollider>().radius;
        moveSpeed = GameObject.Find("Player").GetComponent<wasdMoving>().moveSpeed;
        /*BoxCollider boxCollider = GetComponent<BoxCollider>();
        Vector3 size = boxCollider.size;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (follow)
        {
            dir = target.position - transform.position;
            float speedMultiplier = 0.5f * Mathf.Abs(sphereRange - dir.magnitude);
            transform.position += dir.normalized *( moveSpeed + speedMultiplier+ 0.5f) * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
            Debug.Log(dir.magnitude);
            if (dir.magnitude < 1.5f)
            {
                Destroy(gameObject);
            }
        }
        
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("쫓아가기 시작");
        follow = true;
    }
}
