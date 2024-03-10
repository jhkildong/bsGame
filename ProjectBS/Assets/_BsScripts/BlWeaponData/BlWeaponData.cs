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



    [SerializeField] private string _blweaponzame;          // 축복무기이름
    [SerializeField] private short _id;                     // 아이디
    [SerializeField] private short _attack;                 // 공격력
    [SerializeField] private float _atspeed;                // 공격속도
    [SerializeField] private float _atrange;                // 공격범위
    [SerializeField] private GameObject _blweaponprefab;    // 프리팹
    [SerializeField] private float _projectilesp;           // 투사체 속도

    //public abstract BlWeapon CreateBlWeapon();


}
