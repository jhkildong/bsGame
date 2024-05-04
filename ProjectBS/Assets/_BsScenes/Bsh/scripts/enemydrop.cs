using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
public class enemydrop : MonoBehaviour
{
    public int hp = 10;
    private bool hasDead = false;
    DropTable dropTable;

    void Start()
    {
        dropTable = GetComponent<DropTable>();
    }
    //생사 여부
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
        Debug.Log("부딛혀서 체력이 깎임. 현재 체력 : " + hp);
    }
}
*/