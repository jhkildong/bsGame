using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVanish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �浹 �� 1�� �ڿ� Destroy �޼��� ȣ��
        Invoke("DestroyItem", 1f);
    }

    private void DestroyItem()
    {
        Destroy(gameObject);
    }
}
