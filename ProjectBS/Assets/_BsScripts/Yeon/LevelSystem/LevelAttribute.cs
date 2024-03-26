using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;

namespace Yeon
{
    [Serializable]
    public class LevelAttribute<T>
    {
        static int MaxLevel = 7;
        [SerializeField]private List<Info> LevelUpInfo = new List<Info>();

        [System.Serializable]
        public class Info
        {
            public string Attribute;
            public float[] LevelTable;
            public event UnityAction action;

            public Info(string name)
            {
                Attribute = name;
                LevelTable = new float[MaxLevel];
            }
        }

#if UNITY_EDITOR
        IEnumerable<FieldInfo> TFieldInfos;
        [HideInInspector]public string[] TFieldNames;

        public LevelAttribute()
        {
            if (TFieldInfos != null) return;
            Initilize();
        }

        public void Initilize()
        {
            TFieldInfos = typeof(T).GetFields(BindingFlags.NonPublic | BindingFlags.Instance).
                Where(field => field.FieldType == typeof(int) || field.FieldType == typeof(float) || field.FieldType == typeof(short));
            TFieldNames = new string[TFieldInfos.Count()];
            

            for(int i = 0; i< TFieldInfos.Count(); i++)
            {
                TFieldNames[i] = TFieldInfos.ElementAt(i).Name;
            }
        }

        public void CreateFieldArray(int index)
        {
            if (index < 0 || index >= TFieldInfos.Count())
            {
                Debug.Log($"{index}, {TFieldInfos.Count()}");
                return;
            }

            var fieldInfo = TFieldInfos.ElementAt(index);
            string fieldName = fieldInfo.Name;

            LevelUpInfo.Add(new Info(fieldName));
        }

        public void RemoveFieldArray(int index)
        {
            if (index < 0 || index >= TFieldInfos.Count())
            {
                return;
            }

            var fieldInfo = TFieldInfos.ElementAt(index);
            string fieldName = fieldInfo.Name;
            LevelUpInfo.RemoveAll(info => info.Attribute == fieldName);
        }

        public static explicit operator LevelAttribute<T>(UnityEngine.Object v)
        {
            throw new NotImplementedException();
        }
#endif
    }
}

