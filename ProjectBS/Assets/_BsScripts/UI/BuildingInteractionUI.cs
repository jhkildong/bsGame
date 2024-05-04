using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuildingInteractionUI : FollowingTargetUI
{
    [SerializeField] private GameObject PressR; //수리버튼
    [SerializeField] private GameObject PressG; //파괴버튼

    public GameObject upgradeUI;

    public TextMeshProUGUI reqWood; //업그레이드 필요 재화
    public TextMeshProUGUI reqStone;
    public TextMeshProUGUI reqIron;

    private void OnDisable()
    {
        transform.position = new Vector3(0, 100000, 0);
    }
}
