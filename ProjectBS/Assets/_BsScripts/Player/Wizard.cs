using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wizard : PlayerComponent
{
    private enum EffectType
    {
        Fire = 3520,
        Ice = 3521,
        Lightning = 3522
    }
    const int defaultID = 3520;

    public override Effect[] MyEffects
    {
        get
        {
            if (_effects.Length == 0)
            {
                _effects = Resources.LoadAll<Effect>("Effect/Wizard");
            }
            return _effects;
        }
    }
    [SerializeField] private EffectType _effectType;
    public override Effect GetMyEffect()
    {
        return MyEffects[(int)_effectType - defaultID];
    }

    public void SelectMyEffect(int id)
    {
        _effectType = (EffectType)id;
    }

    public override void OnAttackPoint()
    {
        //공격 이펙트 생성
        
    }
}
