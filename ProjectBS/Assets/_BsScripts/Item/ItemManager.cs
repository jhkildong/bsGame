using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour
{
    //>>>>>>>>>>>>>>>>>SingleTon
    private static ItemManager instance = null;
    private void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
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



    [System.Serializable]
    public class Items
    {
        public ItemData data;
    }
    public List<Items> items = new List<Items>();
    
    public ItemData GetItemDataByID(int id)
    {
        foreach (var item in items)
        {
            if (item.data.ID == id)
            {
                Debug.Log("아이템 데이터 가져옴.");
                return item.data;
            }
        }
        return null; // 해당 ID에 대한 아이템 데이터가 없는 경우 null 반환
    }

    public void A(List<DropTable.dropItem> items)
    {
        float rnd = Random.Range(0, 100);
        foreach(var dropitem in items)
        {
            if(dropitem.dropChance > rnd)
            {
                Drop(GetItemDataByID(dropitem.ID));
                break;
            }
            else
            {
                rnd = rnd - dropitem.dropChance;
            }
        }
    }

    public void Drop(ItemData itemData)
    {
        Debug.Log("아이템 드랍 :" + itemData.Name);
        itemData.CreateItem();
    }
}