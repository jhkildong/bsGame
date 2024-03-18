using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{
    public enum itemtype { None, exp, structure, oneshot}
    //������ ���� Ÿ�� ù�ٿ��� ������.

    public int ID => _id;
    public string Name => _name;
    public float Value => _value;
    public GameObject ItemPrefab => _prefab;
   // public itemtype TypeOfItem => _itemType;


    [SerializeField] private int _id;               
    [SerializeField] private string _name;
    [SerializeField] private float _value;
    [SerializeField] private GameObject _prefab;
    //[SerializeField] private itemtype _itemType;
    // Start is called before the first frame update
    public abstract GameObject CreateItem();
}
