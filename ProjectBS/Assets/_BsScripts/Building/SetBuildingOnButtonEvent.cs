using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetBuildingOnButtonEvent : MonoBehaviour
{
    //��ư�� ����
    // ��ư ������ �ش� �ǹ��� InstantiateBuilding�� GameObject selectBuilding�� ����. (���� Ŀ�ø� ����)
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
