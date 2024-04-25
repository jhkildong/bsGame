using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    Ray ray;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
     Physics.Raycast(new Ray(transform.position, transform.forward),out RaycastHit hit);
        Debug.Log(hit.collider);
    }
}
