using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Yeon
{
    /// <summary>
    /// ���� �ý����� �����ϱ� ���� Ŭ����
    /// 1~7���������� ������ �Ӽ����� �����ϰ�, �������� �Ӽ����� ������Ŵ
    /// �ʱ�ȭ �ϴ� �޼���� Editor������ ���
    /// ScriptableObject���� ����� ����� �Ӽ��� ���� ����
    /// Public Property�� �о���̰�, int, float, short Ÿ�Ը� �о����
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

        /// <summary>
        /// �������� �� �� ������ �Ӽ��� ������ ������ ���� �����ϴ� Ŭ����
        /// </summary>
        [System.Serializable]
        public class LevelTable
        {
            public string Attribute;    //�Ӽ�(������Ƽ) �̸�
            public float[] Table;       //�Ӽ��� ������ ��
            public Action<float> LevelUpAct;    //������ �� ������ �޼���

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
        //�����Ϳ��� ���� ������ ����ȭ�� �����ʴ� �����͵��� ���� �������� ����
        //������Ƽ�� Ÿ�� ������ �����ϰ� ����ȭ ��Ű�� ���� ����ϴ� Ŭ����
        [HideInInspector,SerializeField] List<SerializablePropertyInfo> SerializablePropertyInfos;
        IEnumerable<PropertyInfo> PropertyInfos;
        [HideInInspector]public string[] PropertyNames;

        //ó�� �ʱ�ȭ�� �޼���
        public void Initialize1(object obj)
        {
            _type = obj.GetType();
            //int, float, short Ÿ�Ը� ������(Reflection)
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

        //���� ���������� �ٸ� ��� �޼��� �ʵ��� ������ �ٲ���� ��� ����
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

        //����� ���� �Ӽ����� ����
        public void CreatePropertyArray(int index)
        {
            if (index < 0 || index >= SerializablePropertyInfos.Count())
            {
                Debug.Log("Index Out of Range");
                return;
            }
            //���� ����
            states[index] = true;
            var propertyInfo = SerializablePropertyInfos.ElementAt(index);
            // ������ �Ӽ� �̸��� ���� table ��ü�� ã�� ������ ����
            LevelTable table = LevelTables.Find(i => i.Attribute == propertyInfo.PropertyName);
            if (table == null)
            {
                table = new LevelTable(propertyInfo.PropertyName);
                LevelTables.Add(table);
            }
        }

        //����� ���� �Ӽ����� ����
        public void RemovePropertyArray(int index)
        {
            //���� ����
            states[index] = false;
            var propertyInfo = SerializablePropertyInfos.ElementAt(index);
            LevelTables.RemoveAll(info => info.Attribute == propertyInfo.PropertyName);
        }
#endif
    }

    //������Ƽ�� ������ ����ȭ�ϱ� ���� Ŭ����
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

