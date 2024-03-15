using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemydrop : MonoBehaviour
{
    public int hp = 10;
    private bool hasDead = false;
    public DropTable dropTable; // DropTable을 참조하기 위한 변수

    void Start()
    {
        // DropTable 스크립트가 포함된 게임 오브젝트의 DropTable 컴포넌트 가져오기
        dropTable = GetComponent<DropTable>();
    }
    //생사 여부
    void Update()
    {
        if (hp <= 0 && !hasDead)
        {
            hasDead = true;
            DestroyObject();
            dropTable.WillDrop();
        }
        //죽으면 애니매이션 > 아이템 스폰 > 옵젝 사라짐.      
    }
    void DestroyObject()
    {
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        hp -= 5;
        Debug.Log("부딛혀서 체력이 깎임");
    }
}
