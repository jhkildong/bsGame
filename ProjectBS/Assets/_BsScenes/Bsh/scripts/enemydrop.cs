using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydrop : MonoBehaviour
{
    public int hp = 10;
    private bool hasDead = false;
    DropTable dropTable;

    void Start()
    {
        dropTable = GetComponent<DropTable>();
    }
    //���� ����
    void Update()
    {
        if (hp <= 0)
        {
            //hasDead = true;
            dropTable.WillDrop(dropTable.dropItems).transform.position = this.transform.position + Vector3.up;
            DestroyObject();
        }  
    }
    void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        hp -= 5;
        Debug.Log("�ε����� ü���� ����. ���� ü�� : " + hp);
    }
}
