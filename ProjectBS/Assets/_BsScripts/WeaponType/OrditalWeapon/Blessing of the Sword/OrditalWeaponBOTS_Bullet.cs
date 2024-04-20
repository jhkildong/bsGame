using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrditalWeaponBOTS_Bullet : MonoBehaviour
{
    public LayerMask Monster;

    float Ak;

    private void OnEnable()
    {
        OrditalWeaponBOTS forwardWeaponBOTS = FindObjectOfType<OrditalWeaponBOTS>();
        if (forwardWeaponBOTS != null)
        {
            Ak = forwardWeaponBOTS.Ak;
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
