using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : MonoBehaviour
{

    public ItemManager itemManager;
    private void Start()
    {
        // ItemManager의 인스턴스를 가져와서 참조 설정
        itemManager = ItemManager.Instance;
        if (itemManager == null)
        {
            Debug.LogError("ItemManager is not initialized!");
        }
    }

    [System.Serializable]
    public class dropItem
    {
        public int ID;
        public float dropChance;
    }
    public List<dropItem> dropItems = new List<dropItem>();
    public void WillDrop()
    {
        foreach (var dropItem in dropItems)
        {
            ItemData itemData = itemManager.GetItemDataByID(dropItem.ID);
            if (itemData != null)
            {
                Debug.Log("droptable");
                itemManager.Drop(itemData, dropItem.dropChance);
            }
        }
    }
}