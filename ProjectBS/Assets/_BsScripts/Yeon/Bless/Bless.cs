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


        [SerializeField] private int _id;               // 축복 아이디
        [SerializeField] private string _name;          // 축복 이름
        [SerializeField] private float _attack;         // 축복 공격력
        [SerializeField] private GameObject _prefab;    // 축복 프리팹

        /// <summary> 타입에 맞는 새로운 몬스터 생성 </summary>
        public GameObject CreateClone()
        {
            
        }
    }
}
