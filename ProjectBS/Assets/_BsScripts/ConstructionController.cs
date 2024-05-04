using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionController : MonoBehaviour
{
    [SerializeField]private float buildApplyRange = 2.0f;
    private ConstructionKeyUI buildUI;

    private BuildingInteractionUI buildingInteractionUI;// �Ǽ����� ��ȣ�ۿ� ui ���׷��̵�,���׷��̵� �Ҹ���ȭ, �ı�
    private Transform hitTarget = null;
    private Building buildTarget = null;

    private void Start()
    {
        buildUI = UIManager.Instance.CreateUI(UIID.ConstructionKeyUI, CanvasType.DynamicCanvas) as ConstructionKeyUI;
        UIManager.Instance.SetPool(UIID.ProgressBar, 10, 10);
        buildUI.gameObject.SetActive(false);

        buildingInteractionUI = UIManager.Instance.CreateUI(UIID.BuildingInteractionUI, CanvasType.DynamicCanvas) as BuildingInteractionUI;
        UIManager.Instance.SetPool(UIID.ProgressBar, 10, 10);
        buildingInteractionUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        RaycastHit hit;

        //����ĳ��Ʈ�� �Ǽ��� �� �ִ� ������Ʈ Ž��
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward,
            out hit, buildApplyRange, (int)BSLayerMasks.InCompletedBuilding))
        {
            //���ο� Ÿ���� ��� ����
            if (hitTarget != hit.transform)
            {
                hitTarget = hit.transform;
                buildTarget = hitTarget.GetComponentInChildren<Building>();
                //�Ǽ����� �ƴ� ��� ��ȣ�ۿ�Ű �˾�
                if (!buildTarget.isConstructing)
                {
                    buildUI.gameObject.SetActive(true);
                    buildUI.myTarget = hitTarget;
                }
            }
            //�Ǽ����� ��� ��ȣ�ۿ�Ű �˾� ��Ȱ��ȭ
            if (buildTarget.isConstructing)
            {
                buildUI.gameObject.SetActive(false);
                buildUI.myTarget = null;
                //���� ����
                buildTarget.SelectedProgress?.Invoke(true);
            }
            if (Input.GetKeyDown(KeyCode.B))
            {
                GameManager.Instance.Player.IsBuilding = true;
            }
            if(Input.GetKeyUp(KeyCode.B))
            {
                GameManager.Instance.Player.IsBuilding = false;
            }
            //��ȣ�ۿ�Ű �Է½� �Ǽ�
            if (Input.GetKey(KeyCode.B))
            {
                buildTarget.Construction(GameManager.Instance.Player.ConstSpeed* Time.deltaTime);
            }
            //Ű �Է½� �ı�
            else if (Input.GetKeyDown(KeyCode.G))
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
                //���׷��̵� ���� �ƴҶ� ui�˾�
                if (!buildTarget.isUpgrading)
                {
                    buildingInteractionUI.gameObject.SetActive(true);
                    buildingInteractionUI.myTarget = hitTarget;

                    if (buildTarget._nextUpgrade != null)//���׷��̵� ���� �ǹ��� ���
                    {
                        buildingInteractionUI.upgradeUI.SetActive(true);
                        //�ʿ���ȭ ���̱�
                        buildingInteractionUI.reqWood.text = buildTarget.upgradeWood.ToString();
                        buildingInteractionUI.reqStone.text = buildTarget.upgradeStone.ToString();
                        buildingInteractionUI.reqIron.text = buildTarget.upgradeIron.ToString();
                    }
                    else//�ƴѰ��
                    {
                        buildingInteractionUI.upgradeUI.SetActive(false);
                    }
                }
            }

            //�Ǽ����� ��� ��ȣ�ۿ�Ű �˾� ��Ȱ��ȭ
            if (buildTarget.isUpgrading)
            {
                buildingInteractionUI.gameObject.SetActive(false);
                buildingInteractionUI.myTarget = null;
                //���� ����
                buildTarget.SelectedProgress?.Invoke(true);
            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                GameManager.Instance.Player.IsBuilding = true;
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                GameManager.Instance.Player.IsBuilding = false;
            }
            if (Input.GetKey(KeyCode.R) && buildTarget.CurHp < buildTarget.MaxHp)
            {
                //��������
                buildTarget.Repair(GameManager.Instance.Player.RepairSpeed * Time.deltaTime);
                Debug.Log("����!");
            }


            if (buildTarget._nextUpgrade != null && Input.GetKey(KeyCode.F))
            {
                buildTarget.Upgrade(GameManager.Instance.Player.ConstSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                buildTarget.Destroy();
            }
        }

                //���̿� ������ �ǹ��� ���� ���
        else
        {
            //Ÿ���� �־��� ��� ���� ����
            if(buildTarget != null)
                buildTarget.SelectedProgress?.Invoke(false);
            hitTarget = null;
            buildTarget = null;
            buildUI.gameObject.SetActive(false);
            buildingInteractionUI.gameObject.SetActive(false);
        }
    }
}
