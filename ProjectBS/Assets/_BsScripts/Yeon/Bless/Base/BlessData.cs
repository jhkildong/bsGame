using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bless_", menuName = "Bless", order = 0)]
public class BlessData : ScriptableObject
{
    public int ID => _id;
    public string Name => _name;
    public string Description => _description;
    public GameObject Prefab => _prefab;
    public List<LevelUpData> LvDataList { get => _lvDataList; set => _lvDataList = value; }

    [SerializeField] private int _id;               // �ູ ���̵�
    [SerializeField] private string _name;          // �ູ �̸�
    [SerializeField, Multiline] private string _description;   // �ູ ����
    [SerializeField] private GameObject _prefab;    // �ູ ������
    [SerializeField] private List<LevelUpData> _lvDataList;

    public Bless CreateClone()
    {
        GameObject go = Instantiate(_prefab);
        Transform playerTransform = GameManager.Instance.Player.transform;
        if(playerTransform != null)
            go.transform.SetParent(playerTransform);
        go.transform.localPosition = Vector3.up * 0.7f;
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
        Increase,
        Decrease,
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
            float value = 0;
            for (int i = 0; i < level; i++)
            {
                //���Ҹ� ������ ����(������ �ܸ�)
                if (levelUpType == LevelUpType.Decrease)
                {
                    if (value == 0) value = 1;
                    if (levelUpTable[i] > 1)
                    {
                        //�߸��� ��
                        continue;
                    }
                    value *= (1 - levelUpTable[i]);
                }
                else
                {
                    value += levelUpTable[i];
                }
            }

            switch (levelUpType)
            {
                case LevelUpType.Add:
                    value = defaultValue + value;
                    break;
                case LevelUpType.Increase:
                    value = defaultValue * (1 + value);
                    break;
                case LevelUpType.Decrease:
                    value = defaultValue * value;
                    break;
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

