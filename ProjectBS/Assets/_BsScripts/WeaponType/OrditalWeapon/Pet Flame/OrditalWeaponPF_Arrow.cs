using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OrditalWeaponPF_Arrow : MonoBehaviour
{
    public LayerMask Monster;

    float Ak;

    private void OnEnable()
    {
        OrditalWeaponPF orditalWeaponPF = FindObjectOfType<OrditalWeaponPF>();
        if (orditalWeaponPF != null)
        {
            Ak = orditalWeaponPF.Ak;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((Monster & 1 << other.gameObject.layer) != 0)
        {
            IDamage<Monster> obj = other.GetComponent<IDamage<Monster>>();
            if (obj != null)
            {
                obj.TakeDamage(Ak);
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
        
    }


}
