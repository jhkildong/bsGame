using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_ProjectileAtkStats", menuName = "Building/ProjectileAttackBuildingStats", order = 0)]
public class ProjectileAttackBuildingData : ScriptableObject
{
 
    public float atkPower => _atkPower;
    public float atkSpeed => _atkSpeed;
    public float atkProjectileSize => _atkProjectileSize;
    public float atkProjectileSpeed => _atkProjectileSpeed; // �ǹ� ����ü �ӵ�
    public float atkProjectileRange => _atkProjectileRange; // �ǹ� ����ü ��Ÿ�
    public bool atkCanPen => _atkCanPen; //���밡���� �����ΰ�?
    public int atkPenCount => _atkPenCount; //���밡���� ��ü��
    public LayerMask attackableLayer => _attackableLayer;


    [SerializeField] private float _atkPower;
    [SerializeField] private float _atkSpeed;  // �ǹ��� ���� �ֱ�
    [SerializeField] private float _atkProjectileSize; // �ǹ� ����ü ũ��
    [SerializeField] private float _atkProjectileSpeed;// �ǹ� ����ü �ӵ�
    [SerializeField] private float _atkProjectileRange;// �ǹ� ����ü ��Ÿ�
    [SerializeField] private bool _atkCanPen; //���밡���� �����ΰ�?
    [SerializeField] private int _atkPenCount; //���밡���� ������Ʈ ��
    [SerializeField] private LayerMask _attackableLayer;

}
