using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_ProjectileAtkStats", menuName = "Building/ProjectileAttackBuildingStats", order = 0)]
public class ProjectileAttackBuildingData : ScriptableObject
{
 
    public float atkPower => _atkPower;
    public float atkSpeed => _atkSpeed;
    public float atkProjectileSize => _atkProjectileSize;
    public float atkProjectileSpeed => _atkProjectileSpeed; // 건물 투사체 속도
    public float atkProjectileRange => _atkProjectileRange; // 건물 투사체 사거리
    public bool atkCanPen => _atkCanPen; //관통가능한 공격인가?
    public int atkPenCount => _atkPenCount; //관통가능한 물체수
    public LayerMask attackableLayer => _attackableLayer;


    [SerializeField] private float _atkPower;
    [SerializeField] private float _atkSpeed;  // 건물의 공격 속도
    [SerializeField] private float _atkProjectileSize; // 건물 투사체 크기
    [SerializeField] private float _atkProjectileSpeed;// 건물 투사체 속도
    [SerializeField] private float _atkProjectileRange;// 건물 투사체 사거리
    [SerializeField] private bool _atkCanPen; //관통가능한 공격인가?
    [SerializeField] private int _atkPenCount; //관통가능한 오브젝트 수
    [SerializeField] private LayerMask _attackableLayer;

}
