using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon
{
    /// <summary>
    /// 축복 base 데이터
    /// Bless 스크립트가 바인딩 되어있는 프리팹을 Bless와 Prefab에 지정해줘야함
    /// </summary>
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


        /// <summary>
        /// 클론 생성 prefab이 null이거나 Bless 컴포넌트가 없으면 null 반환
        /// </summary>
        public GameObject CreateClone()
        {
            if(_prefab == null || _prefab.GetComponent<Bless>() == null)
            {
                return null;
            }
            GameObject clone = new GameObject(Name);
            Instantiate(_prefab, clone.transform);
            clone.GetComponentInChildren<Bless>().Init(this);
            return clone;
        }

#if UNITY_EDITOR
        [SerializeField] public LevelProperty LevelAttribute;
#endif
    }
}
