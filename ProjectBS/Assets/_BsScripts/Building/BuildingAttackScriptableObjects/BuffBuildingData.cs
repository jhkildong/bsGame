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
        msBuff
    }
    public string buffName => _buffName; //������
    public float buffAmount => _buffAmount; // ������
    public bool hasDuration => _hasDuration; //������ ���ӽð��� �ִ°�?
    public float duration => _duration; //���ӽð�
    public bool canStack => _canStack; //��ø�� �����Ѱ�?
    public LayerMask targetLayer => _targetLayer;// ������ �ټ��ִ� ���̾��ũ

    public BuffType buffType => _buffType;

    [SerializeField] private string _buffName;
    [SerializeField] private float _buffAmount;
    [SerializeField] private bool _hasDuration;
    [SerializeField] private float _duration;
    [SerializeField] private bool _canStack;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private BuffType _buffType;
}
