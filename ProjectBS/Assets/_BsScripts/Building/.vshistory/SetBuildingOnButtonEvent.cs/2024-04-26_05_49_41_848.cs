using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetBuildingOnButtonEvent : MonoBehaviour
{
    //버튼에 부착
    // 버튼 누르면 해당 건물을 InstantiateBuilding의 GameObject selectBuilding로 전달. (현재 커플링 상태)
    public GameObject building;
    [SerializeField] private int _requireWood;
    [SerializeField] private int _requireStone;
    [SerializeField] private int _requireIron;
    void Start()
    {
        Building myBD = building.GetComponent<Building>();
        _requireWood = myBD.Data.requireWood;
        _requireStone = myBD.Data.requireStone;
        _requireIron = myBD.Data.requireIron;
        
    }

    public void onClickButton()
    {
        //재화 차감


        InstantiateBuilding setBuilding = FindObjectOfType<InstantiateBuilding>(); // ?? 이거 왜 find로 해놨지;;
        setBuilding.selectBuilding = building;
        setBuilding.ChangeStateToBuild();
    }


}
