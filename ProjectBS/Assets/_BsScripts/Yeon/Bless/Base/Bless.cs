using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Yeon
{
    public class Bless : MonoBehaviour
    {   
        public BlessData Data => _data;
        public short Ak => _attack;
        public float Size => _size;
        public short Amount => _amount;
        public short Level => _level;

        [SerializeField]private BlessData _data;
        [SerializeField]private short _attack;
        [SerializeField]private float _size;
        [SerializeField]private short _amount;
        [SerializeField]private short _level;

        public void Init(BlessData data)
        {
            _data = data;
            _attack = data.Ak;
            _size = data.Size;
            _amount = 0;
            _level = 0;
        }
        
    }
}
