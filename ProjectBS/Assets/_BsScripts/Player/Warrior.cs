using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EffectType
{
    Fire = 1000,
    Magic = 1001,
    Snow = 1002,
    Stone = 1003
}

public class Warrior : PlayerComponent
{
    const int defaultID = 1000;

    public override Effect[] MyEffects
    { 
        get
        {
            if (_effects.Length == 0)
            {
                _effects = Resources.LoadAll<Effect>("Effect/Warrior");
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

    private void Start()
    {
        Player myPlayer = GetComponentInParent<Player>();
        MyAnim.GetBehaviour<AttackStateChange>().AttackStateChangeAct += myPlayer.ChangeAttackState;
        MyAnimEvent.ChangeAttackStateAct += myPlayer.ChangeAttackState;
        MyAnimEvent.AttackAct += myPlayer.OnAttackPoint;
    }

}