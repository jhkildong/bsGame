using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScriptweapon : MonoBehaviour
{
    public LayerMask mask;
    public float damaage = 1f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((mask & 1 << other.gameObject.layer) != 0)
        {
            Debug.Log("충돌");
        }
        else
        {
            Debug.Log("몬스터가 아님");
        }
    }
}
