using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ScriptableObject�� ��ӵ� �������̽�
/// </summary>
public interface IPoolable
{
    int ID { get; }
    MonoBehaviour Data { get; }
    IPoolable CreateClone();
}
