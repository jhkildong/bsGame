using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForwardWeaponMoving : MonoBehaviour
{
    public float bulletSpeed = 20.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime); // ¿Ãµø
    }

}
