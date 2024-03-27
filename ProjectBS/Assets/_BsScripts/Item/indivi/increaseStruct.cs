using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class increaseStruct : MonoBehaviour
{
    GameManager gM;
    StructItemData.itemtype itemtype;
    private void Start()
    {
        gM = GameManager.Instance;
        //itemtype = gameObject.
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        gM.AddWood(10);
    }
}
