using UnityEngine;

public class ForwardWeaponLD_Bullet : MonoBehaviour
{
    public LayerMask Monster;
    public float Ak;

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
}
