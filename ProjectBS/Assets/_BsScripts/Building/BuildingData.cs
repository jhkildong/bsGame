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

    [SerializeField] private int _id;               // �ǹ� ID
    [SerializeField] private string _buildingName;  // �ǹ� �̸�
    [SerializeField] private short _maxHp;          // �ǹ� �ִ�ü��
    [SerializeField] private short _curHp;          // �ǹ� ����ü��
    [SerializeField] private short _requireWood;      // ���� �䱸 ��ᰳ��
    [SerializeField] private short _requireStone;      // �� �䱸 ��ᰳ��
    [SerializeField] private short _requireIron;      // ö �䱸 ��ᰳ��

    [SerializeField] private float _constSpeed;     // �ǹ� �ִ�ü��
    [SerializeField] private float _repairSpeed;    // �ǹ� �ִ�ü��
    [SerializeField] private GameObject _buildingPrefab;    // �ǹ� prefab




}
