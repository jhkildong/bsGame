using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class SetBuildingOnButtonEvent : MonoBehaviour
{
    //��ư�� ����
    // ��ư ������ �ش� �ǹ��� InstantiateBuilding�� GameObject selectBuilding�� ����. (���� Ŀ�ø� ����)
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
        //��ȭ ����


        InstantiateBuilding setBuilding = FindObjectOfType<InstantiateBuilding>(); // ?? �̰� �� find�� �س���;;
        setBuilding.selectBuilding = building;
        setBuilding.ChangeStateToBuild();
    }


}
