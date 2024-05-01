using UnityEngine;

public abstract class CharacterComponent : MonoBehaviour
{
    protected virtual void Awake()
    {
        _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
    }
    #region Property
    public Transform MyTransform => this.transform;
    public AnimEvent MyAnimEvent
    {
        get
        {
            if (_animEvent == null)
                TryGetComponent(out _animEvent);
            return _animEvent;
        }
    }
    public Animator MyAnim
    {
        get
        {
            if (_anim == null)
                TryGetComponent(out _anim);
            return _anim;
        }
    }
    public SkinnedMeshRenderer[] Myrenderers
    {
        get
        {
            if (_renderers == null)
                _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            return _renderers;
        }
    }
    #endregion

    #region Private Field
    [SerializeField] protected AnimEvent _animEvent;
    [SerializeField] protected Animator _anim;
    [SerializeField] protected SkinnedMeshRenderer[] _renderers;
    #endregion
}
