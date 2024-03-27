using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemFollow : MonoBehaviour
{
    Transform target;
    Vector3 dir;
    float moveSpeed = 5.0f;
    bool follow = false;
    float sphereRange = 2.0f;
    float willbeDestroy = 0.5f;
    ItemData.itemtype DDDD;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        //sphereRange = GameObject.Find("Player").GetComponent<SphereCollider>().radius;
        //moveSpeed = GameObject.Find("Player").GetComponent<wasdMoving>().moveSpeed;
        //willbeDestroy = GameObject.Find("Player").GetComponent<CapsuleCollider>().radius;
        //DDDD = gameObject.GetComponentInParent<ItemData.itemtype>();
        /*BoxCollider boxCollider = GetComponent<BoxCollider>();
        Vector3 size = boxCollider.size;*/
    }

    // Update is called once per frame
    void Update()
    {
        if (follow)
        {
            dir = target.position - transform.position;
            float speedMultiplier = Mathf.Abs(sphereRange - dir.magnitude) + 0.5f;
            transform.position += dir.normalized *(moveSpeed + Mathf.Pow(speedMultiplier, 2)) * Time.deltaTime;
            transform.position = new Vector3(transform.position.x, 1.0f, transform.position.z);
            if (dir.magnitude < 1.0f + willbeDestroy)
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
    
    void Eat()
    {
        Destroy(gameObject);
        Debug.Log(gameObject);
    }
    
}
