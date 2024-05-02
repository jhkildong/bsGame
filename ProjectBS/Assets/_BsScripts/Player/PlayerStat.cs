using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    public int MaxHp => _maxHp;
    public float Sp => _sp;

    [SerializeField]private int _maxHp;
    [SerializeField]private float _sp;
}
