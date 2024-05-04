using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Bless_", menuName = "Bless/Bless", order = 0)]
public class BlessData : BaseBlessData
{
    public Sprite Icon => _icon;
    public string Name => _name;
    public string Description => _description;
    public GameObject Prefab => _prefab;
    public List<LevelUpData> LvDataList { get => _lvDataList; set => _lvDataList = value; }

    [SerializeField] private Sprite _icon;           // 축복 아이콘
    [SerializeField] private string _name;          // 축복 이름
    [SerializeField, Multiline] private string _description;   // 축복 설명
    [SerializeField] private GameObject _prefab;    // 축복 프리팹
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

    //레벨을 입력하면 자동으로 레벨에 맞는 값을 반환
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
                //감소면 복리로 감소(증가는 단리)
                if (levelUpType == LevelUpType.Decrease)
                {
                    if (value == 0) value = 1;
                    if (levelUpTable[i] > 1)
                    {
                        //잘못된 값
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

