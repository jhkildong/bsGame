using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;

//캐릭터 컴포넌트 관리해주는 클래스
public abstract class PlayerComponent : CharacterComponent
{
    #region Property
    ////////////////////////////////Property////////////////////////////////
    public Transform MyEffectSpawn 
    {
        get
        {
            if (_effectSpawn == null)
            {
                if(_effectSpawn = transform.Find("EffectSpawn"))
                {
                    GameObject go = new GameObject("EffectSpawn");
                    go.transform.SetParent(transform);
                    go.transform.localPosition = new Vector3(0, 0.7f, 1.5f);
                }
            }
            return _effectSpawn;
        }
    }
    public Rig[] MyRigs
    {
        get
        {
            if (_rigs == null)
            {
                if ((_rigs = GetComponentsInChildren<Rig>()) == null)
                    return null;
            }
            return _rigs;
        }
    }
    public abstract Effect[] MyEffects{ get; }
    public float Attack
    {
        get => _attack;
        set => _attack = value;
    }
    #endregion

    #region Private Field
    ////////////////////////////////PrivateField////////////////////////////////
    [SerializeField] protected Transform _effectSpawn;
    [SerializeField] protected Rig[] _rigs;
    [SerializeField] protected Effect[] _effects = new Effect[0];
    [SerializeField] protected float _attack;
    #endregion

    #region Public Method
    ////////////////////////////////PublicMethod////////////////////////////////
    public void SetRigWeight(float weight)
    {
        foreach (Rig rig in MyRigs)
        {
            rig.weight = weight;
        }
    }
    public abstract Effect GetMyEffect();

    public abstract void OnAttackPoint();
    #endregion
}