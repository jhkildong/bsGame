using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class SetBuildingOnButtonEvent : MonoBehaviour
{
    private Button button;
    private Image btnImage;
    private Color btnOrgColor;
    private Color btnColor;
    //버튼에 부착
    // 버튼 누르면 해당 건물을 InstantiateBuilding의 GameObject selectBuilding로 전달. (현재 커플링 상태)
    public GameObject building;
    public TextMeshProUGUI reqWoodText;
    public TextMeshProUGUI reqStoneText;
    public TextMeshProUGUI reqIronText;
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
        
        reqWoodText.text = _requireWood.ToString();
        reqStoneText.text = _requireStone.ToString();
        reqIronText.text = _requireIron.ToString();

        GameManager.Instance.WoodChangeAct += (num) => CanBuild(); //자원이 갱신될때마다 버튼이 활성화 가능한지 점검
        GameManager.Instance.StoneChangeAct += (num) => CanBuild();
        GameManager.Instance.IronChangeAct += (num) => CanBuild();

        CanBuild();
        
    }

    //현재 보유중인 재화가 부족하면 버튼 반투명화, 클릭 불가능. -> 재화가 달라질때마다(ChangeAct가 invoke될대마다) 검사해야함. -> 각changeact 에 추가

    private void CanBuild()
    {
        //재화가 부족한 경우
        if(GameManager.Instance.CurWood() < _requireWood || GameManager.Instance.CurStone()< _requireStone || GameManager.Instance.CurIron()<_requireIron)
        {
            btnColor.a = 0.5f;
            btnImage.color = btnColor; // 반투명하게 변경
            button.interactable = false; // 상호작용 불가능
        }
        else
        {
            btnImage.color = btnOrgColor; // 투명도 복구
            button.interactable = true; // 상호작용 불가능
        }
    }
    public void onClickButton()
    {

        //재화 차감은 좌클릭으로 설치하는 순간에 한다. Building의 OnInstalled
        

        InstantiateBuilding setBuilding = FindObjectOfType<InstantiateBuilding>(); // ?? 이거 왜 find로 해놨지;;
        setBuilding.selectBuilding = building;
        setBuilding.ChangeStateToBuild();
    }


}
