using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPos : MonoBehaviour
{
    [SerializeField] Transform target;

    // Update is called once per frame
    void Update()
    {
        if(target == null)
            return;
        transform.position = target.position;
    }
}
