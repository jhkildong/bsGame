using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTypeParams : MonoBehaviour
{
    //무기 형식 종류
    public short forwardAt { get; set; }    //전방 공격형
    public short orbitalAt { get; set; }    //궤도 공격형
    public short rangeAt { get; set; }      //범위 공격형

    //보조무기 파라미터

    //투사체 무기 인터페이스
    public float projectileSp { get; set; }
}
