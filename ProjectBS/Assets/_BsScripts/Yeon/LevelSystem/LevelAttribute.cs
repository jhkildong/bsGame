using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
using UnityEngine.Events;

namespace Yeon
{
    [Serializable]
    public class LevelAttribute
    {
        static int MaxLevel = 7;
        private int _id;
        private Type _type;
        [SerializeField] private List<Info> LevelUpInfo = new List<Info>();

        [System.Serializable]
        public class Info
        {
            public string Attribute;
            public float[] LevelTable;
            public UnityAction<float> LevelUpAct;

            public Info(string name)
            {
                Attribute = name;
                LevelTable = new float[MaxLevel];
            }
        }

        public void LevelUp(int level)
        {
            foreach (var info in LevelUpInfo)
            {
                info.LevelUpAct?.Invoke(info.LevelTable[level - 1]);
            }
        }

#if UNITY_EDITOR
        IEnumerable<FieldInfo> TFieldInfos;
        IEnumerable<PropertyInfo> TPropertyInfos;
        [HideInInspector] public string[] TFieldNames;
        [HideInInspector] public string[] TPropertyNames;

        public void Initilize(object obj, int id)
        {
            _type = obj.GetType();
            _id = id;
            //int, float, short 타입만 가져옴
            TFieldInfos = _type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance).
                Where(field => field.FieldType == typeof(int) || field.FieldType == typeof(float) || field.FieldType == typeof(short));

            TPropertyInfos = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance).
                Where(property => property.PropertyType == typeof(int) || property.PropertyType == typeof(float) || property.PropertyType == typeof(short));


            TFieldNames = new string[TFieldInfos.Count()];
            TPropertyNames = new string[TPropertyInfos.Count()];

            for (int i = 0; i < TFieldInfos.Count(); i++)
            {
                StringBuilder sb = new StringBuilder("ID:");
                sb.Append(_id.ToString());
                TFieldNames[i] = sb.Append(TFieldInfos.ElementAt(i).Name).ToString();
            }

            for (int i = 0; i < TPropertyInfos.Count(); i++)
            {
                StringBuilder sb = new StringBuilder("ID:");
                sb.Append(_id.ToString());
                TPropertyNames[i] = sb.Append(TPropertyInfos.ElementAt(i).Name).ToString();
            }
        }

        public void CreateFieldArray(int index)
        {
            if (index < 0 || index >= TFieldInfos.Count())
            {
                return;
            }

            var fieldInfo = TFieldInfos.ElementAt(index);
            StringBuilder sb = new StringBuilder("ID:");
            sb.Append(_id.ToString());
            sb.Append(fieldInfo.Name);

            //중복이 있으면 리턴
            foreach (var info in LevelUpInfo)
            {
                if (info.Attribute == sb.ToString())
                {
                    return;
                }
            }
            LevelUpInfo.Add(new Info(sb.ToString()));
        }

        public void CreatePropertyArray(object obj, int index)
        {
            if (index < 0 || index >= TPropertyInfos.Count())
            {
                return;
            }

            var propertyInfo = TPropertyInfos.ElementAt(index);
            StringBuilder sb = new StringBuilder("ID:");
            sb.Append(_id.ToString());
            sb.Append(propertyInfo.Name);
            Info info = new Info(sb.ToString());
            info.LevelUpAct = (val) => propertyInfo.SetValue(obj, val);
            Debug.Log(info.LevelUpAct.GetMethodInfo());
            //중복이 있으면 리턴
            foreach (var lvInfo in LevelUpInfo)
            {
                if (lvInfo.Attribute == sb.ToString())
                {
                    return;
                }
            }

            LevelUpInfo.Add(info);
        }

        public void RemoveFieldArray(int index)
        {
            if (index < 0 || index >= TFieldInfos.Count())
            {
                return;
            }

            var fieldInfo = TFieldInfos.ElementAt(index);
            StringBuilder sb = new StringBuilder("ID:");
            sb.Append(_id.ToString());
            sb.Append(fieldInfo.Name);
            LevelUpInfo.RemoveAll(info => info.Attribute == sb.ToString());
        }

        public void RemovePropertyArray(int index)
        {
            if (index < 0 || index >= TPropertyInfos.Count())
            {
                return;
            }

            var propertyInfo = TPropertyInfos.ElementAt(index);
            StringBuilder sb = new StringBuilder("ID:");
            sb.Append(_id.ToString());
            sb.Append(propertyInfo.Name);
            LevelUpInfo.RemoveAll(info => info.Attribute == sb.ToString());
        }

        public static explicit operator LevelAttribute(UnityEngine.Object v)
        {
            throw new NotImplementedException();
        }
#endif
    }
}

