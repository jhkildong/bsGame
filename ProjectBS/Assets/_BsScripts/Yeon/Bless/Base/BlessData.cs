using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon
{
    /// <summary>
    /// �ູ base ������
    /// Bless ��ũ��Ʈ�� ���ε� �Ǿ��ִ� �������� Bless�� Prefab�� �����������
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

        [SerializeField] private int _id;               // �ູ ���̵�
        [SerializeField] private string _name;          // �ູ �̸�
        [SerializeField] private short _attack;         // �ູ ���ݷ�
        [SerializeField] private float _size;           // �ູ ũ��
        [SerializeField] private GameObject _prefab;    // �ູ ������
        [SerializeField] private Bless _bless;          // �ູ ��ũ��Ʈ


        /// <summary>
        /// Ŭ�� ���� prefab�� null�̰ų� Bless ������Ʈ�� ������ null ��ȯ
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
