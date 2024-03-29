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
        if (other.GetComponent<SphereCollider>() != null)
        {
            originalRadius = other.GetComponent<SphereCollider>().radius;
            //other.GetComponent<SphereCollider>().radius += increaseAmount;
            Debug.Log("Ŀ��");
            //StartCoroutine(ReturnToOriginalSize(other.GetComponent<SphereCollider>()));
        }
    }
    private IEnumerator ReturnToOriginalSize(SphereCollider collider)
    {
        new WaitForSeconds(returnTime);
        Debug.Log("�ڼ��� ������� ���ư�.");
        yield return collider.radius = originalRadius;
    }
}
