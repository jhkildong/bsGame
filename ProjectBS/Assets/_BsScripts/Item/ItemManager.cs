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
        // 충돌한 GameObject에서 ScriptableObject 스크립트를 가져옵니다.
        ItemData scriptableObject = other.gameObject.GetComponent<ItemData>();

        // ScriptableObject이 존재하고, 특정 ID를 가지고 있다면
        if (scriptableObject != null)
        {
            Debug.Log("파괴");
            Destroy(other.gameObject);
        }
    }

    //Tag가 뭔지
    //아이템 이름
    //종류 식별( 어떤 종류 아이템인지)
    //해당 종류 이벤트에 맞는 이벤트
}
