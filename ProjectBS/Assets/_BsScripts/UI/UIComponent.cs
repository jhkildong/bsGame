using UnityEditor;
using UnityEngine;

public class UIComponent : MonoBehaviour, IPoolable
{
    public int ID => _id;
    [SerializeField] private int _id;
    public MonoBehaviour This => this;

    public IPoolable CreateClone()
    {
        return Instantiate(this);
    }
}