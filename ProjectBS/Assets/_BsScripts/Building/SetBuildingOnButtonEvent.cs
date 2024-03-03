using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetBuildingOnButtonEvent : MonoBehaviour
{
    //버튼에 부착
    // 버튼 누르면 해당 건물을 InstantiateBuilding의 GameObject selectBuilding로 전달. (현재 커플링 상태)
    public GameObject building;
    void Start()
    {

    
    }


    public void onClickButton()
    {
        InstantiateBuilding setBuilding = FindObjectOfType<InstantiateBuilding>();
        setBuilding.selectBuilding = building;
        setBuilding.ChangeStateToBuild();
    }


}
