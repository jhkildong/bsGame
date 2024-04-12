using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIPerception : MonoBehaviour
{
    public event UnityAction<Transform> findEnemy;
    public event UnityAction lostEnemy;
    private LayerMask myMask;

    public void Init(LayerMask mask, UnityAction<Transform> find = null, UnityAction lost = null)
    {
        myMask = mask;
        findEnemy += find;
        lostEnemy += lost;
        SphereCollider col = gameObject.AddComponent<SphereCollider>();
        col.radius = 5.0f;
        col.isTrigger = true;
        gameObject.layer = 13; //юс╫ц
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((myMask & (1 << other.gameObject.layer)) != 0)
        {
            findEnemy?.Invoke(other.transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((int)BSLayerMasks.Player & (1 << other.gameObject.layer)) != 0)
        {
            lostEnemy?.Invoke();
        }
    }
}
