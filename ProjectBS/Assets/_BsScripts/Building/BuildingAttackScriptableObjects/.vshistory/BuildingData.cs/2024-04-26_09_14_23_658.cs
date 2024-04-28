using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuildingBase_ScriptableObject", menuName = "Building/BuildingScriptableObject", order = 0)]

public class BuildingData : ScriptableObject
{

    public int ID => _id;
    public string buildingName => _buildingName;
    public short maxHp => _maxHp;
    public short curHp => _curHp;
    public short requireWood => _requireWood;
    public short requireStone => _requireStone;
    public short requireIron => _requireIron;
    public float constTime => _constTime;
    public float repairSpeed => _repairSpeed;

    public GameObject nextUpgrade => _nextUpgrade;


    [SerializeField] private int _id;               // 건물 ID
    [SerializeField] private string _buildingName;  // 건물 이름
    [SerializeField] private short _maxHp;          // 건물 최대체력
    [SerializeField] private short _curHp;          // 건물 현재체력
    [SerializeField] private short _requireWood;      // 나무 요구 재료개수
    [SerializeField] private short _requireStone;      // 돌 요구 재료개수
    [SerializeField] private short _requireIron;      // 철 요구 재료개수
    [SerializeField] private float _constTime;     // 건물 총 건설시간
    [SerializeField] private float _repairSpeed;    // 건물 수리속도

    [SerializeField] private GameObject _nextUpgrade;    // 다음 업그레이드 건물


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
