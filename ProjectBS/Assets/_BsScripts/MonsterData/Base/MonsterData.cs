using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonsterData : ScriptableObject
{
    public int ID => _id;
    public string Name => _name;
    public float Ak => _attack;
    public float AkDelay => _attackDelay;
    public float Sp => _speed;
    public short MaxHP => _maxHp;
    public GameObject MonsterPrefab => _prefab;

    [SerializeField] private int _id;               // ���� ���̵�
    [SerializeField] private string _name;          // ���� �̸�
    [SerializeField] private short _attack;         // ���� ���ݷ�
    [SerializeField] private short _attackDelay;    // ���� ���ݵ�����
    [SerializeField] private short _speed;          // ���� �̵��ӵ�
    [SerializeField] private short _maxHp;          // ���� �ִ�ü��
    [SerializeField] private GameObject _prefab;    // ���� ������

    /// <summary> Ÿ�Կ� �´� ���ο� ���� ���� </summary>
    public abstract Monster CreateMonster();

}
