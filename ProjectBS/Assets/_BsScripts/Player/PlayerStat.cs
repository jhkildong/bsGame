using UnityEngine;

[System.Serializable]
public class PlayerStat
{
    public int MaxHp => _maxHp;
    public float Ak => _ak;
    public float Sp => _sp;
    public float AkSp => _akSp;
    public float SkillDmg => _skillDmg;
    public float SkillCoolTime => _skillCoolTime;

    [SerializeField]private int _maxHp;
    [SerializeField]private int _ak;
    [SerializeField]private float _sp;
    [SerializeField]private float _akSp;
    [SerializeField]private float _skillDmg;
    [SerializeField]private float _skillCoolTime;

    public void UpdateStat(int maxHp, int ak, float sp, float akSp)
    {
        _maxHp = maxHp;
        _ak = ak;
        _sp = sp;
        _akSp = akSp;
    }
}
