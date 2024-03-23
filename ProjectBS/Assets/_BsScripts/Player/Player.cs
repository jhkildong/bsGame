using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Yeon;

public enum BSLayerMasks
{
    Player = 1 << 14,
    Monster = 1 << 15,
    Building = 1 << 24,
    InCompletedBuilding = 1 << 25,
    BuildCheckObject = 1 << 26,
    Ground = 1 << 29
}

public class Player : Combat
{
    [SerializeField] protected Animator myAnim;

    private void InitPlayerSetting()
    {
        myAnim = GetComponentInChildren<Animator>();
        //ChangeHpAct.AddListener(PlayerUI.Instance.ChangeHP);
        CurHp = MaxHP;
        attackMask = (int)BSLayerMasks.Monster;
    }

    protected override void Start()
    {
        base.Start();
        InitPlayerSetting();
    }
}
