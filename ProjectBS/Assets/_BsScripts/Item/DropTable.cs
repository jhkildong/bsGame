using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class dropItem
{
    public int ID;
    public float dropChance;
}

public interface IDropable
{
    List<dropItem> dropItems();
    void WillDrop();
}
public class DropTable : MonoBehaviour
{
    ItemManager itemManager;
    private void Start()
    {
        // ItemManager�� �ν��Ͻ��� �����ͼ� ���� ����
        itemManager = ItemManager.Instance;
        if (itemManager == null)
        {
            Debug.LogError("ItemManager is not initialized!");
        }
        dropItems = GetComponent<IDropable>().dropItems();
    }
    public List<dropItem> dropItems;
    public GameObject WillDrop()
    {
        return itemManager.A(dropItems);
    }
}