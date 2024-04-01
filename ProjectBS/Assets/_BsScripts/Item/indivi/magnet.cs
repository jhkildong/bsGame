using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnet : MonoBehaviour
{
    private float originalRadius;
    private float targetRadius;
    public float increaseAmount = 1.0f;
    public float returnTime = 2f;

    private void OnTriggerEnter(Collider other)
    {
        if ((int)(BSLayerMasks.MagneticField) == (1 << other.gameObject.layer))
        {
            originalRadius = other.GetComponent<SphereCollider>().radius;
            //other.GetComponent<SphereCollider>().radius += increaseAmount;
            Debug.Log("커짐");
            //StartCoroutine(ReturnToOriginalSize(other.GetComponent<SphereCollider>()));
        }
    }
    private IEnumerator ReturnToOriginalSize(SphereCollider collider)
    {
        new WaitForSeconds(returnTime);
        Debug.Log("자성이 원래대로 돌아감.");
        yield return collider.radius = originalRadius;
    }
}
