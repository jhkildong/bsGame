using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class increaseStruct : MonoBehaviour
{
    GameManager gM;
    StructItemData struct_Item_data;
    private void Start()
    {
        gM = GameManager.Instance;
        struct_Item_data = GetComponent<StructItem>().StructItemData;
    }
    private void OnTriggerEnter(Collider other)
    {
        switch (struct_Item_data.TypeOfMaterial)
        {
            case StructItemData.struct_mat.None:
                Debug.Log("�������� ��ᰡ �������� �ʾ���.");
                break;
            case StructItemData.struct_mat.stone:
                gM.ChangeStone(struct_Item_data.Value);
                break;
            case StructItemData.struct_mat.wood:
                gM.ChangeWood(struct_Item_data.Value);
                break;
            case StructItemData.struct_mat.steel:
                gM.ChangeIron(struct_Item_data.Value);
                break;
            //���� �߰� �� ����
        }
        
    }
}