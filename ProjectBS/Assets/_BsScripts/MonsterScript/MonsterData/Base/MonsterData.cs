using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterType
{
    Single, Group, Surround
}
public abstract class MonsterData : ScriptableObject, IPoolable
{
    public int ID => _id;
    public string Name => _name;
    public float Ak => _attack;
    public float AkDelay => _attackDelay;
    public float Sp => _speed;
    public short MaxHP => _maxHp;
    public short Exp => _exp;
    public GameObject Prefab => _prefab;
    
    public List<dropItem> DropItemList => _dropItemList;

    [SerializeField] private int _id;               // ���� ���̵�
    [SerializeField] private string _name;          // ���� �̸�
    [SerializeField] private float _attack;         // ���� ���ݷ�
    [SerializeField] private float _attackDelay;    // ���� ���ݵ�����
    [SerializeField] private float _speed;          // ���� �̵��ӵ�
    [SerializeField] private short _maxHp;          // ���� �ִ�ü��
    [SerializeField] private short _exp;
    [SerializeField] private GameObject _prefab;    // ���� ������
    [SerializeField] private List<dropItem> _dropItemList;

    /// <summary> Ÿ�Կ� �´� ���ο� ���� ���� </summary>
    public abstract GameObject CreateClone();

}
