using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemParams : MonoBehaviour
{
    //������ �Ķ����
    public string itemName {  get; private set; }
    public short ID { get; set; }
    public GameObject itemPrefab { get; set; }

    //����ġ ������
    public short exp {  get; set; }

    //�Ǽ� ��� ������
    public enum matType { Tree, Stone, Bronze, Iron }
    public short matNumber { get; set; }

    //1ȸ�� ������
}
