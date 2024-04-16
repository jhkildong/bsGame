using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Warrior : PlayerComponent
{
    private enum EffectType
    {
        Fire = 3500,
        Magic = 3501,
        Snow = 3502,
        Stone = 3503
    }
    const int defaultID = 3500;

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
        MyAnim.GetBehaviour<ResetDir>().ResetDirAct += ResetAtackDir;
    }
    private bool attackDirSwitch = false;
    private void ChangeAttackDir()
    {
        attackDirSwitch = !attackDirSwitch;
    }
    public void ResetAtackDir()
    {
        attackDirSwitch = false;
    }
   
    public override void OnAttackPoint()
    {
        float _attackDir = attackDirSwitch ? 0f : 180f;
        ChangeAttackDir();

        //공격 이펙트 생성
        GameObject go = ObjectPoolManager.Instance.GetEffect(GetMyEffect(), Attack).This.gameObject;
        go.transform.position = MyEffectSpawn.position;
        go.transform.rotation = Quaternion.Euler(0.0f, MyEffectSpawn.rotation.eulerAngles.y, _attackDir);
    }
}