using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class SetBuildingOnButtonEvent : MonoBehaviour
{
    private Button button;
    private Image btnImage;
    private Color btnOrgColor;
    private Color btnColor;
    //��ư�� ����
    // ��ư ������ �ش� �ǹ��� InstantiateBuilding�� GameObject selectBuilding�� ����. (���� Ŀ�ø� ����)
    public GameObject building;
    [SerializeField] private int _requireWood;
    [SerializeField] private int _requireStone;
    [SerializeField] private int _requireIron;
    void Start()
    {
        button = GetComponent<Button>();
        btnImage = button.GetComponent<Image>();
        btnOrgColor = btnImage.color;
        btnColor = btnOrgColor;


        Building myBD = building.GetComponent<Building>();
        _requireWood = myBD.Data.requireWood;
        _requireStone = myBD.Data.requireStone;
        _requireIron = myBD.Data.requireIron;
        /*
        GameManager.Instance.WoodChangeAct += CanBuild;
        GameManager.Instance.StoneChangeAct += playerUI.ChangeStoneText;
        GameManager.Instance.IronChangeAct += playerUI.ChangeIronText;
        */
        CanBuild();
    }

    //���� �������� ��ȭ�� �����ϸ� ��ư ������ȭ, Ŭ�� �Ұ���. -> ��ȭ�� �޶���������(ChangeAct�� invoke�ɴ븶��) �˻��ؾ���. -> ��changeact �� �߰�

    private void CanBuild()
    {
        //��ȭ�� ������ ���
        if(GameManager.Instance.CurWood() < _requireWood || GameManager.Instance.CurStone()< _requireStone || GameManager.Instance.CurIron()<_requireIron)
        {
            btnColor.a = 0.5f;
            btnImage.color = btnColor; // �������ϰ� ����
            button.interactable = false; // ��ȣ�ۿ� �Ұ���
        }
        else
        {
            btnImage.color = btnOrgColor; // ���� ����
            button.interactable = true; // ��ȣ�ۿ� �Ұ���
        }
    }
    public void onClickButton()
    {
        //��ȭ ����

        GameManager.Instance.ChangeWood(-_requireWood);
        GameManager.Instance.ChangeStone(-_requireStone);
        GameManager.Instance.ChangeIron(-_requireIron);


        InstantiateBuilding setBuilding = FindObjectOfType<InstantiateBuilding>(); // ?? �̰� �� find�� �س���;;
        setBuilding.selectBuilding = building;
        setBuilding.ChangeStateToBuild();
    }


}
