using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class increaseExp : MonoBehaviour
{
    GameManager gM;
    void Start()
    {
        gM = GameManager.Instance;
    }
    private void OnTriggerEnter(Collider other)
    {
        gM.ChangeExp(GetComponent<ExpItem>().ExpItemData.Value);
    }
}
