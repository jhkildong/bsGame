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
        //레이캐스트로 건설할 수 있는 오브젝트 탐색
        if (Physics.Raycast(transform.position + new Vector3(0, 0.1f, 0), transform.forward,
            out RaycastHit hit, buildApplyRange, (int)BSLayerMasks.InCompletedBuilding))
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
            if (Input.GetKeyDown(KeyCode.B))
            {
                GameManager.Instance.Player.IsBuilding = true;
            }
            if(Input.GetKeyUp(KeyCode.B))
            {
                GameManager.Instance.Player.IsBuilding = false;
            }
            //상호작용키 입력시 건설
            if (Input.GetKey(KeyCode.B))
            {
                buildTarget.Construction(Time.deltaTime);
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
        }
    }
}
