using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        // �浹�� GameObject���� ScriptableObject ��ũ��Ʈ�� �����ɴϴ�.
        ItemData scriptableObject = other.gameObject.GetComponent<ItemData>();

        // ScriptableObject�� �����ϰ�, Ư�� ID�� ������ �ִٸ�
        if (scriptableObject != null)
        {
            Debug.Log("�ı�");
            Destroy(other.gameObject);
        }
    }

    //Tag�� ����
    //������ �̸�
    //���� �ĺ�( � ���� ����������)
    //�ش� ���� �̺�Ʈ�� �´� �̺�Ʈ
}
