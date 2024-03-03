using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemParams : MonoBehaviour
{
    //아이템 파라미터
    public string itemName {  get; private set; }
    public short ID { get; set; }
    public GameObject itemPrefab { get; set; }

    //경험치 아이템
    public short exp {  get; set; }

    //건설 재료 아이템
    public enum matType { Tree, Stone, Bronze, Iron }
    public short matNumber { get; set; }

    //1회성 아이템
}
