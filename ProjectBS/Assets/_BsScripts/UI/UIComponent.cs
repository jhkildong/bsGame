using UnityEditor;
using UnityEngine;

public abstract class UIComponent : MonoBehaviour, IPoolable
{
    public abstract int ID { get; }
    public MonoBehaviour This => this;

    public IPoolable CreateClone()
    {
        return Instantiate(this);
    }
}