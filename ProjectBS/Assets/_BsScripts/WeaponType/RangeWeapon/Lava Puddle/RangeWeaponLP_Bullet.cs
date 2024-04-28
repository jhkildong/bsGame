using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeaponLP_Bullet : MonoBehaviour
{
    private LayerMask Monster;

    public float Ak;

    float DelayTime = 0.2f;
    float time;

    // Start is called before the first frame update
    void Start()
    {
        Monster = (int)BSLayerMasks.Monster | (int)BSLayerMasks.SurroundMonster;
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;

        Collider[] list = Physics.OverlapSphere(transform.position, 2.0f, Monster);

        if (time >= DelayTime)
        {
            time = 0.0f;
            foreach (Collider col in list)
            {
                IDamage<Monster> obj = col.GetComponent<IDamage<Monster>>();
                if (obj != null)
                {
                    obj.TakeDamage(Ak);
                    Debug.Log("Attack");
                }
            }
        }
    }
}
