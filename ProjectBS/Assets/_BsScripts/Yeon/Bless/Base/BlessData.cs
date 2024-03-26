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
        public Bless Bless => _bless;

        [SerializeField] private int _id;               // 축복 아이디
        [SerializeField] private string _name;          // 축복 이름
        [SerializeField] private short _attack;         // 축복 공격력
        [SerializeField] private float _size;           // 축복 크기
        [SerializeField] private GameObject _prefab;    // 축복 프리팹
        [SerializeField] private Bless _bless;          // 축복 스크립트

        public LevelAttribute LevelAttribute;

        public GameObject CreateClone()
        {
            GameObject clone = new GameObject(Name);
            clone.AddComponent<Bless>().Init(this);
            return null;
        }
    }
}
