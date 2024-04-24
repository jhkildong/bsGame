using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponLP_Bullet : MonoBehaviour
{
    public LayerMask Monster;
    public float Ak;

    float time = 0.0f;
    float DelayTime = 0.3f;

    private void OnTriggerStay(Collider other) // ´ë¹ÌÁö
    {
        if(time >= DelayTime)
        {
            if ((Monster & 1 << other.gameObject.layer) != 0)
            {
                time = 0.0f;
                IDamage<Monster> obj = other.GetComponent<IDamage<Monster>>();
                if (obj != null)
                {
                    obj.TakeDamage(Ak);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
    }
}
