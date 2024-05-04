using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseBlessData : ScriptableObject
{
    public int ID => _id;
    [SerializeField] private int _id;
}
