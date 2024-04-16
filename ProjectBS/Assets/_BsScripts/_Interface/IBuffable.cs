using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IBuffable
    {
        Buff getBuff { get; set; }
        /*
        void AtkBuff(float buff){}// 공격력 버프
        void HPBuff(float buff) {} // 체력 버프
        void ASBuff(float buff) {} // 공격속도 버프
        void MSBuff(float buff) {} // 이동속도 버프
        */
    }

    public class Buff
    {
        public List<float> aktBuffList;
        public float atkBuff;

        public List<float> hpBuffList;
        public float hpBuff;

        public List<float> asBuffList;
        public float asBuff;
    
        public List<float> msBuffList;
        public float msBuff;
    }
