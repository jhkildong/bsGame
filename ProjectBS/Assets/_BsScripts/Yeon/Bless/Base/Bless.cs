using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Yeon
{
    /// <summary>
    /// �ູ base Ŭ����
    /// Public ������Ƽ�� ������ ���� �߿��� ������ �� �����Ǵ� ������ ������ �� ����
    /// gameObject�� ���ε����� blessData�� �Ҵ� �������
    /// </summary>
    public class Bless : MonoBehaviour
    {
        #region Property
        public BlessData Data => _data;
        public float Ak {get => _attack; set => _attack = value;}
        public float Size { get => _size; set => _size = value; }
        public short Amount { get => _amount; set => _amount = value; }
        public GameObject BlessPrefab { get => _blessPrefab; set => _blessPrefab = value; }
        public LevelProperty LevelProp => _levelProp;
        #endregion

        #region Field
        [SerializeField] protected BlessData _data;
        [SerializeField] protected float _attack;
        [SerializeField] protected float _size;
        [SerializeField] protected short _amount;
        [SerializeField] protected GameObject _blessPrefab;
        //[SerializeField] private short _level;
        [SerializeField, ReadOnly] protected LevelProperty _levelProp;
        #endregion

        public void Init(BlessData data)
        {
            _data = data;
            _attack = data.Ak;
            _size = data.Size;
            _amount = 0;
            //_level = 0;
        }

        public void Awake()
        {
            Init(_data);
        }

    }
}
