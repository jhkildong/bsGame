using UnityEngine;
using UnityEngine.Animations.Rigging;

//ĳ���� ������Ʈ �������ִ� Ŭ����
public abstract class PlayerComponent : CharacterComponent
{
    #region Property
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
    #endregion

    #region Private Field
    [SerializeField] protected Transform _effectSpawn;
    [SerializeField] protected Rig[] _rigs;
    [SerializeField] protected Effect[] _effects = new Effect[0];
    #endregion

    public void SetRigWeight(float weight)
    {
        foreach (Rig rig in MyRigs)
        {
            rig.weight = weight;
        }
    }
    public abstract Effect GetMyEffect();
}