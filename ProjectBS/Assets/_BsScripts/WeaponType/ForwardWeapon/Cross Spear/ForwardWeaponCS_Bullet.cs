using UnityEngine;

public class ForwardWeaponCS_Bullet : MonoBehaviour
{
    public LayerMask Monster;
    public float Ak;

    private void OnTriggerEnter(Collider other) // ´ë¹ÌÁö
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
