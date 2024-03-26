using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Yeon
{
    public class Bless : MonoBehaviour
    {   
        public BlessData Data => _data;
        public float Ak {get => _attack; set => _attack = value;}
        public float Size { get => _size; set => _size = value; }
        public float Amount { get => _amount; set => _amount = value; }
        public LevelAttribute LevelAttribute => _levelAttribute;

        [SerializeField] protected BlessData _data;
        [SerializeField] protected float _attack;
        [SerializeField] protected float _size;
        [SerializeField] protected float _amount;
        [SerializeField] private short _level;
        [SerializeField] protected LevelAttribute _levelAttribute;

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
