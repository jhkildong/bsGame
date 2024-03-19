using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydrop : MonoBehaviour
{
    public int hp = 10;
    private bool hasDead = false;
    public DropTable dropTable;

    void Start()
    {
        // DropTable ��ũ��Ʈ�� ���Ե� ���� ������Ʈ�� DropTable ������Ʈ ��������
        dropTable = GetComponent<DropTable>();
    }
    //���� ����
    void Update()
    {
        if (hp <= 0 && !hasDead)
        {
            hasDead = true;
            dropTable.WillDrop().transform.position = this.transform.position;
            DestroyObject();
        }
        //������ �ִϸ��̼� > ������ ���� > ���� �����.      
    }
    void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        hp -= 5;
        Debug.Log("�ε����� ü���� ����");
    }
}
