using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>������ Ÿ��</summary>
[System.Flags]
public enum MonsterType
{
    Single = 0b_0000_0001,
    Group = 0b_0000_0010,
    Surround = 0b_0000_0100,
    StraightMove = 0b_0000_1000,
}
public abstract class MonsterData : ScriptableObject
{
    public int ID => _id;
    public string Name => _name;
    public float Ak => _attack;
    public float AkDelay => _attackDelay;
    public float Sp => _speed;
    public short MaxHP => _maxHp;
    public short Exp => _exp;
    public float Mass => _mass;
    public float Radius => _radius;
    public GameObject Prefab => _prefab;
    
    public List<dropItem> DropItemList => _dropItemList;
    

    [SerializeField] private int _id;               // ���� ���̵�
    [SerializeField] private string _name;          // ���� �̸�
    [SerializeField] private float _attack;         // ���� ���ݷ�
    [SerializeField] private float _attackDelay;    // ���� ���ݵ�����
    [SerializeField] private float _speed;          // ���� �̵��ӵ�
    [SerializeField] private short _maxHp;          // ���� �ִ�ü��
    [SerializeField] private short _exp;            // ���� ����ġ
    [SerializeField] private float _mass;           // ���� ����
    [SerializeField] private float _radius;         // ���� ũ��
    [SerializeField] private GameObject _prefab;    // ���� ������
    [SerializeField] private List<dropItem> _dropItemList;

    /// <summary> Ÿ�Կ� �´� ���ο� ���� ���� </summary>
    public abstract Monster CreateClone();

}
