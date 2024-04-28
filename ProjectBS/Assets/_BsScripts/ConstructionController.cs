using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ConstructionController : MonoBehaviour
{
    [SerializeField]private float buildApplyRange = 2.0f;
    private ConstructionKeyUI buildUI;
    private Transform hitTarget = null;
    private Building buildTarget = null;

    private void Start()
    {
        buildUI = UIManager.Instance.CreateUI(UIID.ConstructionKeyUI, CanvasType.DynamicCanvas) as ConstructionKeyUI;
        UIManager.Instance.SetPool(UIID.ProgressBar, 10, 10);
        buildUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        //����ĳ��Ʈ�� �Ǽ��� �� �ִ� ������Ʈ Ž��
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward,
            out RaycastHit hit, buildApplyRange, (int)BSLayerMasks.InCompletedBuilding))
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
                buildTarget.Construction(Time.deltaTime);
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
        }
    }
}
