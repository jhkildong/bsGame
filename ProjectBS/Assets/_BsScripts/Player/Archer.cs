using UnityEngine;
using static UnityEditorInternal.ReorderableList;

public class Archer : PlayerComponent
{
    private enum EffectType
    {
        Lightning = 3504
    }
    const int defaultID = 3504;

    public override Effect[] MyEffects
    {
        get
        {
            if (_effects.Length == 0)
            {
                _effects = Resources.LoadAll<Effect>("Effect/Archer");
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
        GameObject go = ObjectPoolManager.Instance.GetEffect(GetMyEffect(), Attack).This.gameObject;
        go.transform.position = MyEffectSpawn.position;
        go.transform.rotation = transform.rotation;
    }
}
