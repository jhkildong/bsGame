using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionController : MonoBehaviour
{
    [SerializeField]private float buildApplyRange = 2.0f;
    private ConstructionKeyUI buildUI;
    private BuildingInteractionUI buildInteractionUI;
    private Transform hitTarget = null;
    private Building buildTarget = null;

    private void Start()
    {
        buildUI = UIManager.Instance.CreateUI(UIID.ConstructionKeyUI, CanvasType.DynamicCanvas) as ConstructionKeyUI;
        UIManager.Instance.SetPool(UIID.ProgressBar, 10, 10);
        buildUI.gameObject.SetActive(false);

        buildInteractionUI = UIManager.Instance.CreateUI(UIID.BuildingInteractionUI, CanvasType.DynamicCanvas) as BuildingInteractionUI;
        buildInteractionUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        RaycastHit hit;

        //����ĳ��Ʈ�� �Ǽ��� �� �ִ� ������Ʈ Ž��
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward,
            out hit, buildApplyRange, ((int)BSLayerMasks.InCompletedBuilding)| (int)BSLayerMasks.Building))
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
            if (Input.GetKeyDown(KeyCode.B)|| Input.GetKeyDown(KeyCode.R))
            {
                GameManager.Instance.Player.IsBuilding = true;
            }
            if(Input.GetKeyUp(KeyCode.B)|| Input.GetKeyUp(KeyCode.R))
            {
                GameManager.Instance.Player.IsBuilding = false;
            }
            //��ȣ�ۿ�Ű �Է½� �Ǽ�
            if (Input.GetKey(KeyCode.B))
            {
                buildTarget.Construction(GameManager.Instance.Player.ConstSpeed* Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.R) && buildTarget.CurHp < buildTarget.MaxHp)
            {
                //��������
                buildTarget.Repair(GameManager.Instance.Player.RepairSpeed * Time.deltaTime);
                Debug.Log("����!");
            }
            //Ű �Է½� �ı�
            else if (Input.GetKeyDown(KeyCode.G))
            {
                buildTarget.Destroy();
            }
        }
        /*
        else if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward,
            out hit, buildApplyRange, (int)BSLayerMasks.Building))
        {

            if(Input.GetKeyDown (KeyCode.R)) 
            {
                GameManager.Instance.Player.IsBuilding = true;
            }
            if(Input.GetKeyDown(KeyCode.R))
            {
                GameManager.Instance.Player.IsBuilding = false;
            }
            if (Input.GetKey(KeyCode.R) && buildTarget.CurHp < buildTarget.MaxHp)
            {
                //��������
                buildTarget.Repair(GameManager.Instance.Player.RepairSpeed*Time.deltaTime);
                Debug.Log("����!");
            }
            //Ű �Է½� �ı�
            else if (Input.GetKeyDown(KeyCode.E))
            {
                buildTarget.Destroy();
            }
        }
        */
        //���̿� ������ �ǹ��� ���� ���
        else
        {
            //Ÿ���� �־��� ��� ���� ����
            if(buildTarget != null)
                buildTarget.SelectedProgress?.Invoke(false);
            hitTarget = null;
            buildTarget = null;
            buildUI.gameObject.SetActive(false);
        }
    }
}
