using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionController : MonoBehaviour
{
    private bool canBuild = true;

    [SerializeField]private float buildApplyRange = 2.0f;
    private ConstructionKeyUI buildUI;

    private BuildingInteractionUI buildingInteractionUI;// 건설이후 상호작용 ui 업그레이드,업그레이드 소모재화, 파괴
    private Transform hitTarget = null;
    private Building buildTarget = null;

    private void Start()
    {
        UIManager.Instance.SetPool(UIID.ProgressBar, 10, 10);

        buildUI = UIManager.Instance.CreateUI(UIID.ConstructionKeyUI, CanvasType.DynamicCanvas) as ConstructionKeyUI;
        buildUI.gameObject.SetActive(false);

        buildingInteractionUI = UIManager.Instance.CreateUI(UIID.BuildingInteractionUI, CanvasType.DynamicCanvas) as BuildingInteractionUI;
        buildingInteractionUI.gameObject.SetActive(false);

        GameManager.Instance.Player.OnSkillAct += () => canBuild = false;
        if (GameManager.Instance.Player.GetComponentInChildren<Archer>() == null)          //임시처리
        {
            GameManager.Instance.Player.OffSkillAct += () => canBuild = true;
        }
        GameManager.Instance.Player.StartAttackAct += () => canBuild = false;
        GameManager.Instance.Player.EndAttackAct += () => canBuild = true;


    }
    private void Update()
    {
        RaycastHit hit;

        if (!canBuild)
            return;

        //레이캐스트로 건설할 수 있는 오브젝트 탐색
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward,
            out hit, buildApplyRange, (int)BSLayerMasks.InCompletedBuilding))
        {
            //새로운 타겟일 경우 갱신
            if (hitTarget != hit.transform)
            {
                hitTarget = hit.transform;
                buildTarget = hitTarget.GetComponentInChildren<Building>();
                //건설중이 아닐 경우 상호작용키 팝업
                if (!buildTarget.isConstructing)
                {
                    buildUI.gameObject.SetActive(true);
                    buildUI.myTarget = hitTarget;
                }
            }
            //건설중일 경우 상호작용키 팝업 비활성화
            if (buildTarget.isConstructing)
            {
                buildUI.gameObject.SetActive(false);
                buildUI.myTarget = null;
                //색깔 변경
                buildTarget.SelectedProgress?.Invoke(true);
            }
            if(Input.GetKeyUp(KeyCode.B))
            {
                GameManager.Instance.Player.IsBuilding = false;
            }
            //상호작용키 입력시 건설
            if (Input.GetKey(KeyCode.B))
            {
                GameManager.Instance.Player.IsBuilding = true;
                buildTarget.Construction(GameManager.Instance.Player.ConstSpeed* Time.deltaTime);
            }
            //키 입력시 파괴
            if (Input.GetKeyDown(KeyCode.G))
            {
                buildTarget.Destroy();
            }
        }
        
        else if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward,
            out hit, buildApplyRange, (int)BSLayerMasks.Building))
        {
            if (hitTarget != hit.transform)
            {
                hitTarget = hit.transform;
                buildTarget = hitTarget.GetComponentInChildren<Building>();
                //업그레이드 중이 아닐때 ui팝업
                if (!buildTarget.isUpgrading)
                {
                    buildingInteractionUI.gameObject.SetActive(true);
                    buildingInteractionUI.myTarget = hitTarget;

                    if (buildTarget._nextUpgrade != null)//업그레이드 가능 건물일 경우
                    {
                        buildingInteractionUI.upgradeUI.SetActive(true);
                        //필요재화 보이기
                        buildingInteractionUI.reqWood.text = buildTarget.upgradeWood.ToString();
                        buildingInteractionUI.reqStone.text = buildTarget.upgradeStone.ToString();
                        buildingInteractionUI.reqIron.text = buildTarget.upgradeIron.ToString();
                    }
                    else//아닌경우
                    {
                        buildingInteractionUI.upgradeUI.SetActive(false);
                    }
                }
            }

            //건설중일 경우 상호작용키 팝업 비활성화
            if (buildTarget.isUpgrading)
            {
                buildingInteractionUI.gameObject.SetActive(false);
                buildingInteractionUI.myTarget = null;
                //색깔 변경
                buildTarget.SelectedProgress?.Invoke(true);
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                GameManager.Instance.Player.IsBuilding = false;
            }
            if (Input.GetKey(KeyCode.R) && buildTarget.CurHp < buildTarget.MaxHp)
            {
                GameManager.Instance.Player.IsBuilding = true;
                //수리구현
                buildTarget.Repair(GameManager.Instance.Player.RepairSpeed * Time.deltaTime);
                Debug.Log("수리!");
            }


            if (buildTarget._nextUpgrade != null && Input.GetKeyDown(KeyCode.F))
            {
                buildTarget.StartUpgrade();
            }
            if (buildTarget._nextUpgrade != null && buildTarget.isUpgrading&& Input.GetKey(KeyCode.B))
            {
                GameManager.Instance.Player.IsBuilding = true;
                buildTarget.Upgrade(GameManager.Instance.Player.ConstSpeed * Time.deltaTime);
            }
            if (Input.GetKeyUp(KeyCode.B))
            {
                GameManager.Instance.Player.IsBuilding = false;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                buildTarget.Destroy();
            }
        }

                //레이에 감지된 건물이 없을 경우
        else
        {
            //타겟이 있었을 경우 색깔 변경
            if(buildTarget != null)
                buildTarget.SelectedProgress?.Invoke(false);
            hitTarget = null;
            buildTarget = null;
            buildUI.gameObject.SetActive(false);
            buildingInteractionUI.gameObject.SetActive(false);
        }
    }
}
