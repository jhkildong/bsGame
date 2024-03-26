using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class heartUp : MonoBehaviour
{
    private float originalRadius; // ���� ������ ��
    public float increaseAmount = 1.0f; // ������ ��
    // Start is called before the first frame update
    void Start()
    {
        originalRadius = GetComponent<SphereCollider>().radius;
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<SphereCollider>().radius += increaseAmount;
        Debug.Log("�ڼ��� �ö�.");

        // ���� �ð��� ���� �� �������� ���� ũ��� ������ �ڷ�ƾ ����
        StartCoroutine(ResetRadius(other.GetComponent<SphereCollider>()));
    }
    private IEnumerator ResetRadius(SphereCollider collider)
    {
        yield return new WaitForSeconds(5.0f); // 5�� ��

        // �������� ���� ũ��� ����
        collider.radius = originalRadius;
        Debug.Log("�ڼ��� ������� ���ƿ�.");
    }
}
