using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrditalWeaponBullet : MonoBehaviour
{
    public float rotSpeed = 1000;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up * rotSpeed * Time.deltaTime);
    }
}
