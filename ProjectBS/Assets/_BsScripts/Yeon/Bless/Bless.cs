using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon
{
    public class BlessData : ScriptableObject
    {
        public int ID => _id;
        public string Name => _name;
        public float Ak => _attack;
        public GameObject Prefab => _prefab;


        [SerializeField] private int _id;               // �ູ ���̵�
        [SerializeField] private string _name;          // �ູ �̸�
        [SerializeField] private float _attack;         // �ູ ���ݷ�
        [SerializeField] private GameObject _prefab;    // �ູ ������

        /// <summary> Ÿ�Կ� �´� ���ο� ���� ���� </summary>
        public GameObject CreateClone()
        {
            
        }
    }
}
