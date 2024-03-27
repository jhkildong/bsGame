using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

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
            public Action<float> LevelUpAct;

            public Info(string name, Action<float> action)
            {
                Attribute = name;
                LevelTable = new float[MaxLevel];
                LevelUpAct += action;
            }
            public void LevelUp(int level)
            {
                Debug.Log(LevelTable[level - 1]);
                LevelUpAct?.Invoke(LevelTable[level - 1]);
            }
        }

        public void LevelUp(int level)
        {
            if (level < 0 || level >= MaxLevel)
            {
                Debug.Log("Level Out of Range");
                return;
            }

            foreach (var info in LevelUpInfo)
            {
                info.LevelUp(level);
            }
        }   
#if UNITY_EDITOR
        IEnumerable<PropertyInfo> TPropertyInfos;
        [HideInInspector] public string[] TPropertyNames;


        public void Initilize(object obj, int id)
        {
            _type = obj.GetType();
            _id = id;
            //int, float, short 타입만 가져옴
            TPropertyInfos = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance).
                Where(property => property.PropertyType == typeof(int) || property.PropertyType == typeof(float) || property.PropertyType == typeof(short));

            TPropertyNames = new string[TPropertyInfos.Count()];

            for (int i = 0; i < TPropertyInfos.Count(); i++)
            {
                StringBuilder sb = new StringBuilder("ID:");
                sb.Append(_id.ToString());
                TPropertyNames[i] = sb.Append(TPropertyInfos.ElementAt(i).Name).ToString();
            }
        }

        public void CreatePropertyArray(object obj, int index)
        {
            if (index < 0 || index >= TPropertyInfos.Count())
            {
                Debug.Log("Index Out of Range");
                return;
            }
            var propertyInfo = TPropertyInfos.ElementAt(index);
            StringBuilder sb = new StringBuilder("ID:");
            sb.Append(_id.ToString());
            sb.Append(propertyInfo.Name);
            string attributeName = sb.ToString();

            // 동일한 속성 이름을 가진 Info 객체를 찾음
            Info info = LevelUpInfo.Find(i => i.Attribute == attributeName);

            if (info == null)
            {
                // Info 객체가 없으면 새로 생성
                info = new Info(attributeName, (float val) =>
                {
                    propertyInfo.SetValue(obj, val);
                    Debug.Log(propertyInfo.GetValue(obj));
                });
                LevelUpInfo.Add(info);
            }
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

