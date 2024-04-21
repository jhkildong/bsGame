using System.Collections;
using System.Collections.Generic;
using UnityEngine;

    public interface IBuffable
    {
        Buff getBuff { get; set; }
    }

    public class Buff
    {
        public List<float> atkBuffList = new List<float>();
        public Dictionary<string, float> atkBuffDict = new Dictionary<string,float>();
        public float atkBuff = 1.0f;//���ݷ� ����

        public List<float> hpBuffList = new List<float>();
        public Dictionary<string, float> hpBuffDict = new Dictionary<string, float>();
        public float hpBuff = 1.0f;//ü�� ����

        public List<float> asBuffList = new List<float>();
        public Dictionary<string, float> asBuffDict = new Dictionary<string, float>();
        public float asBuff = 1.0f; //���� ����
    
        public List<float> msBuffList = new List<float>();
        public Dictionary<string, float> msBuffDict = new Dictionary<string, float>();
        public float msBuff = 1.0f; //�̼� ����

    }
