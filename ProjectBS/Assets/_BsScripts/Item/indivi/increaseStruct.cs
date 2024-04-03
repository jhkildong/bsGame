using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class increaseStruct : MonoBehaviour
{
    GameManager gM;
    StructItemData.struct_mat struct_Mat;
    private void Start()
    {
        gM = GameManager.Instance;
        struct_Mat = GetComponent<StructItem>().StructItemData.TypeOfMaterial;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        switch (struct_Mat)
        {
            case StructItemData.struct_mat.None:
                Debug.Log("�������� ��ᰡ �������� �ʾ���.");
                break;
            case StructItemData.struct_mat.stone:
                gM.ChangeStone(10);
                break;
            case StructItemData.struct_mat.wood:
                gM.ChangeWood(10);
                break;
            case StructItemData.struct_mat.steel:
                gM.ChangeIron(10);
                break;
            //���� �߰� �� ����
        }
    }
}