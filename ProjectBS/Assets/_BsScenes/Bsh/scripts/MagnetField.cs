using UnityEngine;

public class MagnetField : MonoBehaviour
{
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 2f * transform.localScale.x);
    }
#endif

    private void OnTriggerEnter(Collider other)
    {
        if ((int)(BSLayerMasks.Item) == (1 << other.gameObject.layer))
        {
            Item item = other.GetComponent<Item>();
            if (item != null)
            {
                item.Follow(transform);
            }
        }
    }
}
