using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingInteractionUI : FollowingTargetUI
{
    [SerializeField] private GameObject PressR; //������ư
    [SerializeField] private GameObject PressG; //�ı���ư

    public GameObject upgradeUI;

    public TextMeshProUGUI reqWood; //���׷��̵� �ʿ� ��ȭ
    public TextMeshProUGUI reqStone;
    public TextMeshProUGUI reqIron;

    private void OnDisable()
    {
        transform.position = new Vector3(0, 100000, 0);
    }
}
