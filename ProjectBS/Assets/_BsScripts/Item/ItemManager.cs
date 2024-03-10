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
        /*ExpItemData scriptableObject = other.gameObject.GetComponent<ExpItemData>();
        if (scriptableObject != null)
        {
            Debug.Log("아이템과 부딛힘");
        }*/
    }

    //Tag가 뭔지
    //아이템 이름
    //종류 식별( 어떤 종류 아이템인지)
    //해당 종류 이벤트에 맞는 이벤트
}
