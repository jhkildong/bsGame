using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Yeon
{
    /// <summary>
    /// 축복 base 클래스
    /// Public 프로퍼티로 설정된 값들 중에서 레벨업 시 증가되는 값들을 설정할 수 있음
    /// gameObject에 바인딩시켜 blessData에 할당 해줘야함
    /// </summary>
    public class Bless : MonoBehaviour
    {
        #region Property
        public BlessData Data => _data;
        public float Ak {get => _attack; set => _attack = value;}
        public float Size { get => _size; set => _size = value; }
        public float Amount { get => _amount; set => _amount = value; }
        public LevelProperty LevelProp => _levelProp;
        #endregion

        #region Field
        [SerializeField] protected BlessData _data;
        [SerializeField] protected float _attack;
        [SerializeField] protected float _size;
        [SerializeField] protected float _amount;
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
        
    }
}
