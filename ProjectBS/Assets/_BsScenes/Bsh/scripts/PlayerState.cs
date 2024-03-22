using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public float CurrentHp = 100;
    public float Mag;
    public float expMulti = 1.0f;
    private void Start()
    {
        Mag = GetComponent<SphereCollider>().radius;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
