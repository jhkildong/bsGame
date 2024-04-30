using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInteractionUI : FollowingTargetUI
{
    public override int ID => _id;
    [SerializeField] private int _id;

    [SerializeField] private GameObject PressR; //수리버튼
    [SerializeField] private GameObject PressG; //파괴버튼


    private void OnDisable()
    {
        transform.position = new Vector3(0, 100000, 0);
    }
}
