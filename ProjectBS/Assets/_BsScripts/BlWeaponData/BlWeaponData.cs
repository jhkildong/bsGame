using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Playables;

public class BlWeaponData : ScriptableObject
{
    public string BlWeaponName => _blweaponzame;
    public short ID => _id;
    public short Attack => _attack;
    public float AtSpeed => _atspeed;
    public float AtRange => _atrange;
    public GameObject BlWeaponPrefab => _blweaponprefab;
    public float ProjectileSp => _projectilesp;
    public short Level { get; set; }



    [SerializeField] private string _blweaponzame;          // �ູ�����̸�
    [SerializeField] private short _id;                     // ���̵�
    [SerializeField] private short _attack;                 // ���ݷ�
    [SerializeField] private float _atspeed;                // ���ݼӵ�
    [SerializeField] private float _atrange;                // ���ݹ���
    [SerializeField] private GameObject _blweaponprefab;    // ������
    [SerializeField] private float _projectilesp;           // ����ü �ӵ�

    //public abstract BlWeapon CreateBlWeapon();


}
