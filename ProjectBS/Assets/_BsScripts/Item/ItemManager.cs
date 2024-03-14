using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ItemManager : MonoBehaviour
{
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
    [System.Serializable]
    public class Items
    {
        public ItemData name;
        public ItemData data;
        public float weight;
    }
    public List<Items> items = new List<Items>();

    protected ItemData PickItem()
    {
        float sum = 0.0f;
        foreach (var data in items)
        {
            sum += data.weight;
        }

        var rnd = Random.Range(0, sum);

        for (int i = 0; i < items.Count; i++)
        {
            var item = items[i];
            if (item.weight > rnd) return items[i].data;
            else rnd -= item.weight;
        }
        return null;
    }
    public void ItemDrop()
    {
        ItemData item = PickItem();
        if (item == null) return;

        item.CreateItem();
    }
}