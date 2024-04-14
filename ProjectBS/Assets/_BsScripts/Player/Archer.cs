using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : PlayerComponent
{
    public override Effect[] MyEffects
    {
        get
        {
            if (_effects.Length == 0)
            {
                //_effects = Resources.LoadAll<Effect>("Effect/Warrior");
            }
            return _effects;
        }
    }
    [SerializeField] private EffectType _effectType;

    public override Effect GetMyEffect()
    {
        return null;
    }

    public void SelectMyEffect(int id)
    {
        _effectType = (EffectType)id;
    }
}
