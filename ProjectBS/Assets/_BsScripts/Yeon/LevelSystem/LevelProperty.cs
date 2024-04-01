using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Yeon
{
    /// <summary>
    /// 레벨 시스템을 구현하기 위한 클래스
    /// 1~7레벨까지의 레벨당 속성값을 저장하고, 레벨업시 속성값을 증가시킴
    /// 초기화 하는 메서드는 Editor에서만 사용
    /// ScriptableObject에서 토글을 사용해 속성값 설정 가능
    /// Public Property만 읽어들이고, int, float, short 타입만 읽어들임
    /// </summary>
    [Serializable]
    public class LevelProperty
    {
        static int MaxLevel = 7;
        private Type _type;
        public bool[] States => states;
        public List<LevelTable> LevelTables => _levelTables;

        [SerializeField, HideInInspector] private bool[] states;
        [SerializeField] private List<LevelTable> _levelTables = new List<LevelTable>();
        /*
        public void SetAct()
        {
            if (states.Length != SerializablePropertyInfos.Count)
                return;

            for (int i = 0; i < states.Length; i++)
            {
                if (states[i])
                {
                    var propertyInfo = SerializablePropertyInfos[i];
                    LevelTables.Find(info => info.Attribute == PropertyNames[i]).LevelUpAct = (float value) =>
                    {
                        Type type = Type.GetType(propertyInfo.TypeName);
                        object obj = GameObject.Find("GameObject").GetComponent(type);
                        propertyInfo.PropertyInfo.SetValue(obj, value);
                    };
                }
            }
        }
        */

        /// <summary>
        /// 레벨업을 할 때 증가될 속성의 정보와 레벨당 값을 저장하는 클래스
        /// </summary>
        [System.Serializable]
        public class LevelTable
        {
            public string Attribute;    //속성(프로퍼티) 이름
            public float[] Table;       //속성의 레벨당 값
            public Action<float> LevelUpAct;    //레벨업 시 실행할 메서드

            public LevelTable(string name)
            {
                Attribute = name;
                Table = new float[MaxLevel];
            }

            public void LevelUp(int level)
            {
                LevelUpAct?.Invoke(Table[level - 1]);
            }
        }

        public void LevelUp(int level)
        {
            if (level < 0 || level >= MaxLevel)
                return;

            foreach (var info in LevelTables)
            {
                info.LevelUp(level);
            }
        }


#if UNITY_EDITOR
        //에디터에서 값을 설정시 직렬화가 되지않는 데이터들은 값이 유지되지 않음
        //프로퍼티의 타입 정보를 저장하고 직렬화 시키기 위해 사용하는 클래스
        [HideInInspector,SerializeField] List<SerializablePropertyInfo> SerializablePropertyInfos;
        IEnumerable<PropertyInfo> PropertyInfos;
        [HideInInspector]public string[] PropertyNames;

        //처음 초기화시 메서드
        public void Initialize1(object obj)
        {
            _type = obj.GetType();
            //int, float, short 타입만 가져옴(Reflection)
            PropertyInfos = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance).
                Where(property => property.PropertyType == typeof(int) || property.PropertyType == typeof(float) || property.PropertyType == typeof(short));

            SerializablePropertyInfos = PropertyInfos.Select(propertyInfo => new SerializablePropertyInfo
            {
                TypeName = _type.Name,
                PropertyName = propertyInfo.Name
            }).ToList();

            PropertyNames = new string[SerializablePropertyInfos.Count()];
            states = new bool[SerializablePropertyInfos.Count()];
            for (int i = 0; i < SerializablePropertyInfos.Count(); i++)
            {
                PropertyNames[i] = SerializablePropertyInfos.ElementAt(i).PropertyName;
                states[i] = false;
            }
        }

        //값이 존재하지만 다를 경우 메서드 필드의 내용이 바뀌었을 경우 실행
        public void Initialize2(object obj)
        {
            _type = obj.GetType();
            PropertyInfos = _type.GetProperties(BindingFlags.Public | BindingFlags.Instance).
                Where(property => property.PropertyType == typeof(int) || property.PropertyType == typeof(float) || property.PropertyType == typeof(short));

            SerializablePropertyInfos = PropertyInfos.Select(propertyInfo => new SerializablePropertyInfo
            {
                TypeName = _type.AssemblyQualifiedName,
                PropertyName = propertyInfo.Name
            }).ToList();

            string[] strings = new string[SerializablePropertyInfos.Count()];
            bool[] bools = new bool[SerializablePropertyInfos.Count()];
            for (int i = 0; i < SerializablePropertyInfos.Count(); i++)
            {
                strings[i] = SerializablePropertyInfos.ElementAt(i).PropertyName;
                bools[i] = false;
            }
            if(!Enumerable.SequenceEqual(strings, PropertyNames))
            {
                PropertyNames = strings;
                states = bools;
            }
        }

        //토글을 통해 속성값을 설정
        public void CreatePropertyArray(int index)
        {
            if (index < 0 || index >= SerializablePropertyInfos.Count())
            {
                Debug.Log("Index Out of Range");
                return;
            }
            //상태 저장
            states[index] = true;
            var propertyInfo = SerializablePropertyInfos.ElementAt(index);
            // 동일한 속성 이름을 가진 table 객체를 찾고 없으면 생성
            LevelTable table = LevelTables.Find(i => i.Attribute == propertyInfo.PropertyName);
            if (table == null)
            {
                table = new LevelTable(propertyInfo.PropertyName);
                LevelTables.Add(table);
            }
        }

        //토글을 통해 속성값을 해제
        public void RemovePropertyArray(int index)
        {
            //상태 저장
            states[index] = false;
            var propertyInfo = SerializablePropertyInfos.ElementAt(index);
            LevelTables.RemoveAll(info => info.Attribute == propertyInfo.PropertyName);
        }
#endif
    }

    //프로퍼티의 정보를 직렬화하기 위한 클래스
    [Serializable]
    public class SerializablePropertyInfo
    {
        public string TypeName;
        public string PropertyName;

        [NonSerialized]
        private PropertyInfo _propertyInfo;

        public PropertyInfo PropertyInfo
        {
            get
            {
                if (_propertyInfo == null)
                {
                    var type = Type.GetType(TypeName);
                    _propertyInfo = type.GetProperty(PropertyName);
                }
                return _propertyInfo;
            }
        }
    }
}

