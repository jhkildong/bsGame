using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BD_BuffBuilding", menuName = "Building/BuffBuildingData", order = 0)]
public class BuffBuildingData : ScriptableObject
{
    public enum BuffType
    {
        Heal,
        atkBuff,
        hpBuff,
        asBuff,
        msBuff,
        rangeBuff
    }
    public string buffName => _buffName; //버프명
    public float buffAmount => _buffAmount; // 버프량
    public bool hasDuration => _hasDuration; //버프에 지속시간이 있는가?
    public float duration => _duration; //지속시간
    public bool canStack => _canStack; //중첩이 가능한가?
    public LayerMask targetLayer => _targetLayer;// 버프를 줄수있는 레이어마스크

    public BuffType buffType => _buffType;

    [SerializeField] private string _buffName;
    [SerializeField] private float _buffAmount;
    [SerializeField] private bool _hasDuration;
    [SerializeField] private float _duration;
    [SerializeField] private bool _canStack;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private BuffType _buffType;
}
