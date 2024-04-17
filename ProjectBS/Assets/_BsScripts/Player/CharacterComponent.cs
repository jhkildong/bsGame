using UnityEngine;

public abstract class CharacterComponent : MonoBehaviour
{
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
    public SkinnedMeshRenderer Myrenderer
    {
        get
        {
            if (_renderer == null)
                _renderer = GetComponentInChildren<SkinnedMeshRenderer>();
            return _renderer;
        }
    }
    #endregion

    #region Private Field
    [SerializeField] protected AnimEvent _animEvent;
    [SerializeField] protected Animator _anim;
    [SerializeField] protected SkinnedMeshRenderer _renderer;
    #endregion

}
