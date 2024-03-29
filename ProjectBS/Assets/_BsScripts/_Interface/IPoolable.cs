using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject�� ��ӵ� �������̽�
/// </summary>
public interface IPoolable
{
    int ID { get; }
    GameObject CreateClone();
}

public interface IPoolable<T> : IPoolable
{

}