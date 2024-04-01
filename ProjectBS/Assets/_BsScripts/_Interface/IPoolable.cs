using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject에 상속될 인터페이스
/// </summary>
public interface IPoolable
{
    int ID { get; }
    GameObject CreateClone();
}

public interface IPoolable<T> : IPoolable
{

}