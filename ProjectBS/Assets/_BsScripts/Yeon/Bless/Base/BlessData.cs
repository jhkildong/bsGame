using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yeon2
{
    [CreateAssetMenu(fileName = "Bless2_", menuName = "Bless2", order = 0)]
    public class BlessData : ScriptableObject
    {
        public int ID => _id;
        public string Name => _name;
        public GameObject Prefab => _prefab;
        public List<LevelUpData> LvDataList { get => _lvDataList; set => _lvDataList = value; }

        [SerializeField] private int _id;               // �ູ ���̵�
        [SerializeField] private string _name;          // �ູ �̸�
        [SerializeField] private GameObject _prefab;    // �ູ ������
        [SerializeField] private List<LevelUpData> _lvDataList;

        public Bless CreateClone()
        {
            GameObject go = Instantiate(_prefab, GameManager.Instance.playerTransform);
            Bless bless = go.GetComponent<Bless>();
            if (bless != null)
                bless.Init(this);
            return bless;
        }
    }

    [System.Serializable]
    public class LevelUpData
    {
        public const int MAX_LEVEL = 7;
        public enum LevelUpType
        {
            Add,
            Multiply,
        }
        public string name;
        public float defaultValue;
        public LevelUpType levelUpType;
        public float[] levelUpTable = new float[MAX_LEVEL];

        //������ �Է��ϸ� �ڵ����� ������ �´� ���� ��ȯ
        public float this[int level]
        {
            get
            {
                if (level < 0)
                    return 0;
                if (level >= MAX_LEVEL)
                    level = MAX_LEVEL;
                float value = defaultValue;
                for (int i = 0; i < level; i++)
                {
                    switch (levelUpType)
                    {
                        case LevelUpType.Add:
                            value += levelUpTable[i];
                            break;
                        case LevelUpType.Multiply:
                            value *= levelUpTable[i];
                            break;
                    }
                }
                return value;
            }
        }

        public LevelUpData()
        {
            name = "";
            defaultValue = 0;
            levelUpType = LevelUpType.Add;
            levelUpTable = new float[MAX_LEVEL];
        }
    }
}
