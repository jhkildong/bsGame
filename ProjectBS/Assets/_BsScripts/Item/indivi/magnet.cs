using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class magnet : MonoBehaviour
{
    private float originalRadius;
    public float increaseAmount = 1.0f;

    private void OnTriggerEnter(Collider other)
    {
        originalRadius = other.GetComponent<SphereCollider>().radius;
        Debug.Log(originalRadius);
        other.GetComponent<SphereCollider>().radius += increaseAmount;
        Debug.Log("�ڼ��� �ö�.");
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<SphereCollider>().radius = originalRadius;
        Debug.Log("�ڼ��� ������� ���ư�.");
    }
}
