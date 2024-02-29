using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParams : MonoBehaviour
{
    //캐릭터 파라미터
    public string userName { get; private set; }
    public enum job { LongSwordWarrior, FireWizard, Archer, Gambler }
    public short level { get; set;}
    public int exp { get; set;}
    public short posMat { get; set;}

    //캐릭터 스텟 파라미터
    public short maxHp { get; set;}
    public short curHp { get; set;}
    public short attack { get; set;}
    public short defense { get; set;}
    public float speed { get; set;}
    public short crit { get; set;}
    public short critDmg { get; set;}
    public short bossDmg { get; set;}
    public short expBonus { get; set;}
    public float constSpeed { get; set;}
    public float repairSpeed { get; set;}
    public float getitemRange { get; set;}
}


