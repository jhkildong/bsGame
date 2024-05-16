using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class dropItem
{
    public int ID;
    public float dropChance;
}

public class ItemManager : MonoBehaviour
{
    //>>>>>>>>>>>>>>>>>SingleTon
    private static ItemManager instance = null;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static ItemManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    //SingleTon<<<<<<<<<<<<<<<<<<<<<

    [SerializeField, ReadOnly]
    ItemData[] itemDatas;
    Dictionary<int, Item> itemDic = new Dictionary<int, Item>();
    List<Item> DropedItems = new List<Item>();
    
    private void Start()
    {
        itemDatas = Resources.LoadAll<ItemData>(FilePath.Items);
        DataSetPool(itemDatas);
    }

    void DataSetPool(ItemData[] datas)
    {
        foreach (ItemData data in datas)
        {
            if (data.ItemPrefab == null)
            {
                Debug.Log("ItemData Prefab is null");
                continue;
            }
            Item item = data.CreateItem();   //�����͸� ������� �纻����

            itemDic.Add(data.ID, item);                   //�纻�� ����Ʈ�� �߰�(�߰� ȣ���� ����)
            ObjectPoolManager.Instance.SetPool(item, 50, 50);
            ObjectPoolManager.Instance.ReleaseObj(item);
        }
    }

    public GameObject DropRandomItem(List<dropItem> items)
    {
        float rnd = Random.Range(0, 100);
        foreach(var dropitem in items)
        {
            if(dropitem.dropChance > rnd)
            {
                GameObject item = ObjectPoolManager.Instance.GetObj(itemDic[dropitem.ID]).This.gameObject;
                DropedItems.Add(item.GetComponent<Item>());
                return item;
            }
            else
            {
                rnd -= dropitem.dropChance;
            }
        }
        return null;
    }

    public GameObject DropExp(float exp)
    {
        ExpItem item = ObjectPoolManager.Instance.GetObj(itemDic[2500]) as ExpItem;
        item.Exp = (int)exp;
        DropedItems.Add(item.GetComponent<Item>());
        return item.This.gameObject;
    }

    public GameObject DropGold(float gold)
    {
        float rnd = Random.Range(0, 100);
        if (rnd < 40)
        {
            GoldItem item = ObjectPoolManager.Instance.GetObj(itemDic[2600]) as GoldItem;
            item.Gold = (int)gold;
            DropedItems.Add(item.GetComponent<Item>());
            return item.This.gameObject;
        }
        else
            return null;
    }

    public void ReleaseItem(Item item)
    {
        DropedItems.Remove(item);
    }

    public void EatAllItem()
    {
        foreach (Item item in DropedItems)
        {
            item.Follow(GameManager.Instance.Player.transform);
        }
    }
}