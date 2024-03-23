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

       public short attackPower => _attackPower;
       public float attackDelay => _attackDelay;
       public float attackRadius => _attackRadius;
       public float attackProjectileSize => _attackProjectileSize;


       public GameObject buildingPrefab => _buildingPrefab;

       public LayerMask layerMask => _layerMask;

       [SerializeField] private int _id;               // �ǹ� ID
       [SerializeField] private string _buildingName;  // �ǹ� �̸�
       [SerializeField] private short _maxHp;          // �ǹ� �ִ�ü��
       [SerializeField] private short _curHp;          // �ǹ� ����ü��
       [SerializeField] private short _requireWood;      // ���� �䱸 ��ᰳ��
       [SerializeField] private short _requireStone;      // �� �䱸 ��ᰳ��
       [SerializeField] private short _requireIron;      // ö �䱸 ��ᰳ��
       [SerializeField] private float _constTime;     // �ǹ� �� �Ǽ��ð�
       [SerializeField] private float _repairSpeed;    // �ǹ� �����ӵ�

       [SerializeField] private short _attackPower;
       [SerializeField] private float _attackDelay;  // �ǹ��� ���� ������
       [SerializeField] private short _attackRadius;
       [SerializeField] private float _attackProjectileSize;

       [SerializeField] private GameObject _buildingPrefab;    // �ǹ� prefab
       [SerializeField] private LayerMask _layerMask;

       
    /*
    [SerializeField] public int id;               // �ǹ� ID
    [SerializeField] public string buildingName;  // �ǹ� �̸�
    [SerializeField] public short maxHp;          // �ǹ� �ִ�ü��
    [SerializeField] public short curHp;          // �ǹ� ����ü��
    [SerializeField] public short requireWood;      // ���� �䱸 ��ᰳ��
    [SerializeField] public short requireStone;      // �� �䱸 ��ᰳ��
    [SerializeField] public short requireIron;      // ö �䱸 ��ᰳ��

    [SerializeField] public float constTime;     // �ǹ� �� �Ǽ��ð�
    [SerializeField] public float repairSpeed;    // �ǹ� �����ӵ�

    [SerializeField] public short attackPower;
    [SerializeField] public float attackDelay;  // �ǹ��� ���� ������
    [SerializeField] public short attackRadius;
    [SerializeField] public float attackProjectileSize;

    [SerializeField] public GameObject buildingPrefab;    // �ǹ� prefab
    [SerializeField] public LayerMask layerMask;
    */
}
