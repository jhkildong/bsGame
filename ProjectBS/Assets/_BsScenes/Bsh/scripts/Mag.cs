using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mag : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // �浹�� ��� ������Ʈ�� ������
        Collider[] colliders = Physics.OverlapSphere(transform.position, 30f);

        // ��� ������Ʈ�� ��ȸ�ϸ� item ���̾ ���� ������Ʈ�� ã��
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Item"))
            {
                // itemfollow ������Ʈ�� Ȱ��ȭ��Ŵ
                Debug.Log(collider);
                //PlayerA playerA = other.GetComponent<PlayerA>();
                //collider.GetComponent<ItemFollow>().follow(playerA);
            }
        }
    }
}
