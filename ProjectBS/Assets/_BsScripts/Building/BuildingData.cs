using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class BuildingData : ScriptableObject
{
    public int ID => _id;
    public string buildingName => _buildingName;
    public short maxHp => _maxHp;
    public short curHp => _curHp;
    public short requireWood => _requireWood;
    public short requireStone => _requireStone;
    public short requireIron => _requireIron;
    public float constSpeed => _constSpeed;
    public float repairSpeed => _repairSpeed;   
    public GameObject buildingPrefab => _buildingPrefab;

    [SerializeField] private int _id;               // 건물 ID
    [SerializeField] private string _buildingName;  // 건물 이름
    [SerializeField] private short _maxHp;          // 건물 최대체력
    [SerializeField] private short _curHp;          // 건물 현재체력
    [SerializeField] private short _requireWood;      // 나무 요구 재료개수
    [SerializeField] private short _requireStone;      // 돌 요구 재료개수
    [SerializeField] private short _requireIron;      // 철 요구 재료개수

    [SerializeField] private float _constSpeed;     // 건물 최대체력
    [SerializeField] private float _repairSpeed;    // 건물 최대체력
    [SerializeField] private GameObject _buildingPrefab;    // 건물 prefab




}
