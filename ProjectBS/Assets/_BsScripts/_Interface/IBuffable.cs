using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IBuffable
    {
        Buff getBuff { get; set; }
        /*
        void AtkBuff(float buff){}// ���ݷ� ����
        void HPBuff(float buff) {} // ü�� ����
        void ASBuff(float buff) {} // ���ݼӵ� ����
        void MSBuff(float buff) {} // �̵��ӵ� ����
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
