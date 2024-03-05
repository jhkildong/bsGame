using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ItemData : ScriptableObject
{

    public int ID => _id;
    public string Name => _name;
    public float Value => _value;
    public GameObject ItemPrefab => _prefab;


    [SerializeField] private int _id;               
    [SerializeField] private string _name;
    [SerializeField] private float _value;
    [SerializeField] private GameObject _prefab;
    // Start is called before the first frame update
    public abstract Item CreateItem();
}
