using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public enum itemtype { None, exp, structure, oneshot, gold}
    //아이템 종류 타입 첫줄에서 골라야함.

    public int ID => _id;
    public string Name => _name;
    public int Value => _value;
    public GameObject ItemPrefab => _prefab;
    // public itemtype TypeOfItem => _itemType;
    [Multiline]
    public string description;

    [SerializeField] private int _id;               
    [SerializeField] private string _name;
    [SerializeField] private int _value;
    [SerializeField] private GameObject _prefab;
    //[SerializeField] private itemtype _itemType;
    // Start is called before the first frame update
    public abstract Item CreateItem();
}
