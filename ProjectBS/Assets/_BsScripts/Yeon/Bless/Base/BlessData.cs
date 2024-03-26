using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon
{

    [CreateAssetMenu(fileName = "Bless_", menuName = "Bless/Bless", order = 0)]
    public class BlessData : ScriptableObject
    {

        public int ID => _id;
        public string Name => _name;
        public short Ak => _attack;
        public float Size => _size;
        public GameObject Prefab => _prefab;

        [SerializeField] private int _id;               // �ູ ���̵�
        [SerializeField] private string _name;          // �ູ �̸�
        [SerializeField] private short _attack;         // �ູ ���ݷ�
        [SerializeField] private float _size;           // �ູ ũ��
        [SerializeField] private GameObject _prefab;    // �ູ ������

#if UNITY_EDITOR
        public LevelAttribute<BlessData> LevelAttributes = new LevelAttribute<BlessData>();
#endif

        public GameObject CreateClone()
        {
            GameObject clone = new GameObject(Name);
            clone.AddComponent<Bless>().Init(this);
            return null;
        }
    }
}
