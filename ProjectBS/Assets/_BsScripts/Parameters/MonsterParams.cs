using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterParams : MonoBehaviour
{
    //���� �Ķ����
    public string monster {  get; private set; }
    public short ID { get; set; }
    public short attack {  get; set; }
    public float atDelay { get; set; }
    public float speed { get; set; }
    public short maxHP { get; set; }
    public short curHP { get; set; }
    public short defence { get; set; }
    public short buildingDmg { get; set; }
    public GameObject monsterPrefab { get; set; }

    //���� ���� �Ķ����
}
