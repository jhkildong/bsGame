using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParams : MonoBehaviour
{
    //���� �Ķ����
    public string weaponName {  get; private set; }
    public short ID { get; set; }
    public short attack { get; set; }
    public float atSpeed { get; set; }
    public float atRange { get; set; }
    public GameObject weaponPrefab { get; set; }

    //�ֹ��� �Ķ����
    public enum job { LongSwordWarrior, FireWizard, Archer, Gambler }
    public short level { get; set; }

    //�������� �Ķ����

    //����ü ���� �������̽�
    public float projectileSp {  get; set; }

}
