using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[CreateAssetMenu]
public class ItemManager : MonoBehaviour
{

    [System.Serializable]
    public class Items
    {
        public ItemBasic ItemBasic;
        public int weight;
    }
    public ItemData data;

    public List<Items> items = new List<Items>();

    protected ItemBasic PickItem()
    {
        int sum = 0;
        foreach (var ItemBasic in items)
        {
            sum += ItemBasic.weight;
        }

        var rnd = Random.Range(0, sum);

        for (int i = 0; i < items.Count; i++)
        {
            var ItemBasic = items[i];
            if (ItemBasic.weight > rnd) return items[i].ItemBasic;
            else rnd -= ItemBasic.weight;
        }

        return null;
    }

    public void ItemDrop(Vector3 pos)
    {
        GameObject item = data.CreateItem().gameObject;
        if (item == null) return;

        item.transform.position = pos;
    }
}