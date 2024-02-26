using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowingCamera : MonoBehaviour
{
    public Transform myTarget;
    Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - myTarget.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = myTarget.position + offset;
    }
}
