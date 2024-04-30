using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingInteractionUI : FollowingTargetUI
{
    public override int ID => _id;
    [SerializeField] private int _id;

    [SerializeField] private GameObject PressR; //������ư
    [SerializeField] private GameObject PressG; //�ı���ư


    private void OnDisable()
    {
        transform.position = new Vector3(0, 100000, 0);
    }
}
