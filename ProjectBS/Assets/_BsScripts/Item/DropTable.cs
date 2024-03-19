using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropTable : MonoBehaviour
{

    public ItemManager itemManager;
    private void Start()
    {
        // ItemManager�� �ν��Ͻ��� �����ͼ� ���� ����
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
    public GameObject WillDrop()
    {
        return itemManager.A(dropItems);
    }
}