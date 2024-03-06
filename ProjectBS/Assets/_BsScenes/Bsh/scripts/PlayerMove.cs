using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMove : MonoBehaviour
{
    public LayerMask clickMask;
    public LayerMask attackMask;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 1000.0f, clickMask))
            {
                transform.position = hit.point;
            }
        }

        /*if (Input.GetKey(KeyCode))
        {
            transform.position += transform.forward * 2.0f * Time.deltaTime;
        }*/
    }
}
