using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemVanish : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // 충돌 후 1초 뒤에 Destroy 메서드 호출
        Invoke("DestroyItem", 1f);
    }

    private void DestroyItem()
    {
        Destroy(gameObject);
    }
}
