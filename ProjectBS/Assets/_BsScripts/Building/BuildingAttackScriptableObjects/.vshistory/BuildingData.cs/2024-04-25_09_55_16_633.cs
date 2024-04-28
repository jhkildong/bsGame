using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingBase_ScriptableObject", menuName = "Building/BuildingScriptableObject", order = 0)]

public class BuildingData : ScriptableObject
{
    /*
    public int ID => _id;
    public string buildingName => _buildingName;
    public short maxHp => _maxHp;
    public short curHp => _curHp;
    public short requireWood => _requireWood;
    public short requireStone => _requireStone;
    public short requireIron => _requireIron;
    public float constTime => _constTime;
    public float repairSpeed => _repairSpeed;

    public short atkPower => _atkPower;
    public float atkDelay => _atkDelay;
    public float hitDelay => _hitDelay;
    public float atkDuration => _atkDuration;


    public float atkRadius => _atkRadius;
    public float atkProjectileSize => _atkProjectileSize;
    public float atkProjectileSpeed => _atkProjectileSpeed; // 건물 투사체 속도
    public float atkProjectileRange => _atkProjectileRange; // 건물 투사체 사거리
    public bool atkCanPen => _atkCanPen; //관통가능한 공격인가?
    public int atkPenCount => _atkPenCount; //관통가능한 물체수


    public GameObject buildingPrefab => _buildingPrefab;

    public LayerMask attackableLayer => _attackableLayer;

    [SerializeField] private int _id;               // 건물 ID
    [SerializeField] private string _buildingName;  // 건물 이름
    [SerializeField] private short _maxHp;          // 건물 최대체력
    [SerializeField] private short _curHp;          // 건물 현재체력
    [SerializeField] private short _requireWood;      // 나무 요구 재료개수
    [SerializeField] private short _requireStone;      // 돌 요구 재료개수
    [SerializeField] private short _requireIron;      // 철 요구 재료개수
    [SerializeField] private float _constTime;     // 건물 총 건설시간
    [SerializeField] private float _repairSpeed;    // 건물 수리속도

    [SerializeField] private short _atkPower;
    [SerializeField] private float _atkDelay;  // 건물의 공격 주기
    [SerializeField] private float _hitDelay; // 건물공격의 타격 간격(장판형 공격의 경우)
    [SerializeField] private float _atkDuration; // 건물 공격의 지속시간 (장판형 공격의 경우)
    [SerializeField] private short _atkRadius; // 지점형 공격의 범위

    [SerializeField] private float _atkProjectileSize; // 건물 투사체 크기
    [SerializeField] private float _atkProjectileSpeed;// 건물 투사체 속도
    [SerializeField] private float _atkProjectileRange;// 건물 투사체 사거리
    [SerializeField] private bool _atkCanPen; //관통가능한 공격인가?
    [SerializeField] private int _atkPenCount; //관통가능한 오브젝트 수


    [SerializeField] private GameObject _buildingPrefab;    // 건물 prefab
    [SerializeField] private LayerMask _attackableLayer;
    */

    public int ID => _id;
    public string buildingName => _buildingName;
    public short maxHp => _maxHp;
    public short curHp => _curHp;
    public short requireWood => _requireWood;
    public short requireStone => _requireStone;
    public short requireIron => _requireIron;
    public float constTime => _constTime;
    public float repairSpeed => _repairSpeed;

    public GameObject buildingPrefab => _buildingPrefab;


    [SerializeField] private int _id;               // 건물 ID
    [SerializeField] private string _buildingName;  // 건물 이름
    [SerializeField] private short _maxHp;          // 건물 최대체력
    [SerializeField] private short _curHp;          // 건물 현재체력
    [SerializeField] private short _requireWood;      // 나무 요구 재료개수
    [SerializeField] private short _requireStone;      // 돌 요구 재료개수
    [SerializeField] private short _requireIron;      // 철 요구 재료개수
    [SerializeField] private float _constTime;     // 건물 총 건설시간
    [SerializeField] private float _repairSpeed;    // 건물 수리속도

    [SerializeField] private GameObject _buildingPrefab;    // 건물 prefab


    /*
    [SerializeField] public int id;               // 건물 ID
    [SerializeField] public string buildingName;  // 건물 이름
    [SerializeField] public short maxHp;          // 건물 최대체력
    [SerializeField] public short curHp;          // 건물 현재체력
    [SerializeField] public short requireWood;      // 나무 요구 재료개수
    [SerializeField] public short requireStone;      // 돌 요구 재료개수
    [SerializeField] public short requireIron;      // 철 요구 재료개수

    [SerializeField] public float constTime;     // 건물 총 건설시간
    [SerializeField] public float repairSpeed;    // 건물 수리속도

    [SerializeField] public short attackPower;
    [SerializeField] public float attackDelay;  // 건물의 공격 딜레이
    [SerializeField] public short attackRadius;
    [SerializeField] public float attackProjectileSize;

    [SerializeField] public GameObject buildingPrefab;    // 건물 prefab
    [SerializeField] public LayerMask layerMask;
    */
}
