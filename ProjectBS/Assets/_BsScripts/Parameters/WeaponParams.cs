using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParams : MonoBehaviour
{
    //무기 파라미터
    public string weaponName {  get; private set; }
    public short ID { get; set; }
    public short attack { get; set; }
    public float atSpeed { get; set; }
    public float atRange { get; set; }
    public GameObject weaponPrefab { get; set; }

    //주무기 파라미터
    public enum job { LongSwordWarrior, FireWizard, Archer, Gambler }
    public short level { get; set; }

    //보조무기 파라미터

    //투사체 무기 인터페이스
    public float projectileSp {  get; set; }

}
