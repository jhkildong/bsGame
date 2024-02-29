using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingParams : MonoBehaviour
{
    //건물 파라미터
    public string buildingName {  get; private set; }
    public short ID { get; set; }
    public GameObject buildingPrefab { get; set; }
    public short maxHP { get; set; }
    public short curHP { get; set; }
    public short demandMat { get; set; }
    public float constSpeed { get; set; }
    public float repairspeed { get; set; }

    //공격형 건물 파라미터
    public short attck { get; set; }
    public float atDelay { get; set; }
    public short atRange { get; set; }
}
