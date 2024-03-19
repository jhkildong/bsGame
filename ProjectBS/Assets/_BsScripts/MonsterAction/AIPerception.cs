using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AIPerception : MonoBehaviour
{
    public UnityAction<Transform> findEnemy;
    public UnityAction lostEnemy;
    public LayerMask myMask;
    public Transform myTarget;

    public void Init(LayerMask mask, UnityAction<Transform> find = null, UnityAction lost = null)
    {
        myMask = mask;
        myTarget = null;
        findEnemy += find;
        lostEnemy += lost;
    }

    private void OnTriggerEnter(Collider other)
    {
        if ((myMask & (1 << other.gameObject.layer)) != 0)
        {
            if (myTarget == null)
            {
                myTarget = other.transform;
                findEnemy?.Invoke(myTarget);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (myTarget == other.transform)
        {
            myTarget = null;
            lostEnemy?.Invoke();
        }
    }
}
