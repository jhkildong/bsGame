using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class InstantiateBuilding : MonoBehaviour
{
    //public BuildingScriptableObject setBuildingEvent;

    //��� : UI��ưŬ����, SetBuilding.cs���� ChangeStateToBuild�� ȣ���Ͽ� Build���·� ��ȯ.
    //       ���콺 ��ġ�� Ŭ���� �ǹ��� �ش��ϴ� ������ �ǹ��� �����ϰ�, ���콺�� ����ٴϴ� �浹���� ������Ʈ�� SetActive�Ѵ�.
    //       �ش� �ǹ��� collider�� �ִ� ���� ��ġ�� �Ұ����ϸ�(ground�� raycast�� �˻�)
    //       Ŭ���� �ش� �ǹ��� ��ġ�� instantiate�Ѵ�.
    //       �ǹ��� IncompletedBuilding layer ���·� �����Ǹ�, �ش� ���̾�� �浹�� �Ͼ�� �ʴ´�.
    public LayerMask mouseLayer;
    Rigidbody rigid;
    public GameObject selectBuilding; //������ �ǹ� (UI ��ư�� ������ �ش� �ǹ��� ����� �ԷµǾ����)
    public GameObject selectedBuilding;//�Ǽ���ġ�� ���ϱ� ���� ���콺�� ����ٴ� ǥ�ø� ���� �ӽ� �ǹ�
    Collider selectedBuildingCollider;//�Ǽ��� �ӽ� �ǹ��� �ݶ��̴�
    BoxCollider checkBuildingCollider; // �Ǽ����� ��ġ�� ������ ������Ʈ�� Collider
    public GameObject checkBuilding; //�Ǽ� ���� ��ġ�� ������ ������Ʈ
    public GameObject instBuilding; //������ ������ �ǹ�
    bool isBuildReady;
    public bool canBuild;
    Renderer[] orgBuildingRenderer;

    //public event UnityAction BuildingInstalled;
    enum State
    {
        Normal,
        Build
    }

    State state;
    public Vector3 mousePos;
    void Start()
    {
        state = State.Normal;
        canBuild = true;
    }



    // Update is called once per frame
    void Update()
    {
        if (state == State.Normal) { 
           
        }
        if (state == State.Build) // UIŬ���� Build�� �Ѿ�´�. (Button Event�� ����)
        {
            if(!isBuildReady) 
            {
                GetReadyToBuild(); //�Ǽ� �����·�.
                orgBuildingRenderer = selectBuilding.GetComponentsInChildren<Renderer>(); //���� ������ ����,���� �����صα�.
            }
            mousePos = Input.mousePosition;
            Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(mouseRay, out RaycastHit hit, 1000f, mouseLayer)) // ���콺��ġ�� ������ ������Ʈ�� �����ش�. (�ӽ� ������Ʈ�� ����ٴѴ�)
            {
                //Debug.Log(hit.point);
                checkBuilding.transform.position = hit.point;
                //selectedBuilding.transform.position = selectedBuildingTransform.position;
                //selectedBuilding.transform.rotation = selectedBuildingTransform.rotation;
            }
            float wheel = -Input.GetAxis("Mouse ScrollWheel") * 300; //���콺 �� ������ *300(ȸ���ӵ�)
            checkBuilding.transform.rotation *= Quaternion.Euler(0, wheel, 0);// ���콺 �� ������ŭ y�� �������� ȸ���Ѵ�.

            //�Ǽ� ����,�Ұ��� ���θ� ���͸��� ����� ��Ÿ���ִ� �ڵ�
            GameObject _selectedBuilding = selectedBuilding;
            Renderer[] selectedBuildingRenderers = _selectedBuilding.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in selectedBuildingRenderers)
            {
                if (canBuild)
                {
                    renderer.material.color = new Color(0, 0.8f, 0, 0.5f); // ���, ���� 0.5
                }
                else if (!canBuild)
                {
                    renderer.material.color = new Color(1f, 0, 0, 0.5f); //����, ���� 0.5
                }
            }
            


            if (Input.GetMouseButtonDown(0)&&canBuild) // �Ǽ������� ��ġ�� Ŭ����.  //���� ���� ��������(���� ���ϴ±���) ray�� Vector3.zero�� ��ȯ�Ѵ�. (hit.point != Vector3.zero) �ӽ� ����
            {
                instBuilding = selectedBuilding; //������ �ǹ��� ������ �ǹ��̴�.
                Renderer[] instBuildingRenderers = instBuilding.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < orgBuildingRenderer.Length; i++)
                {
                    instBuildingRenderers[i].material.color = orgBuildingRenderer[i].sharedMaterial.color; // ���� �ǹ� �������� �ٽ� ����
                }
                foreach (Renderer renderer in instBuildingRenderers)
                {
                    renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.3f); //���� 0.5
                }
                selectedBuildingCollider.enabled = true; // �ڽ� �ݶ��̴� �ٽ� �ѱ�.(�ݶ��̴��� ������ Layer�� IncompletedBuilding �̱⶧���� ������ �浹 �Ұ���)
                //rigid.isKinematic = false; // isKinematic�� �ٽ� ����.
                //�ǹ��� ��ġ�Ǹ鼭, instBuilding�� ���� ������Ʈ Building�� OnInstalled�� ȣ��Ǿ���Ѵ�. (�ǹ��� ��ġ�� ���·� ����) (Ŀ�ø� �߻� �������̽��� ������ ����)
                Building isInstalled  = instBuilding.GetComponent<Building>();
                isInstalled.OnInstalled();
                Instantiate(instBuilding, hit.point, checkBuilding.transform.rotation); //�ش� ��ġ�� ȸ���� �״�� ����
                EndBuild();
                
            }
            else if (Input.GetMouseButtonDown(0) && !canBuild)
            {
                Debug.Log("�ش� ��ġ�� �Ǽ��Ҽ� �����ϴ�");
            }
            else if (Input.GetMouseButtonDown(1)) // ��Ŭ�� �Է½� �Ǽ� ���
            {
                EndBuild();
            }
        }       
    }

    /*
    public void IsInstalled() // �ǹ��� ���õɶ�(Ŭ������ �Ǽ���ġ�� ��������)
    {
        //BuildingInstalled?.Invoke();
    }
    */

    void ChangeState(State st)
    {
        if (st == state) return;

        else if (st == State.Normal)
        {
            state = State.Normal;
        }
        else if (st == State.Build)
        {
            state = State.Build;
        }

    }

    public void ChangeStateToNormal()
    {
        ChangeState(State.Normal);
    }

    public void ChangeStateToBuild()
    {
        ChangeState(State.Build);
    }

    public void CanBuild()
    {
        canBuild = true;
    }
    public void CantBuild()
    {
        canBuild = false;
    }

    public void SetBuilding(GameObject bd)
    {

    }

    public void EndBuild()
    {
        isBuildReady = false; // �Ǽ������� ��.
        ChangeState(State.Normal);//�Ϲݻ��·� ���ư���.
        Destroy(selectedBuilding);//�ӽ� ������Ʈ �ı�
        checkBuilding.SetActive(false);//�ݶ��̴� üũ ������Ʈ ��Ȱ��ȭ
    }

    public void GetReadyToBuild()
    {
        
        //�Ǽ� ��ư�� ������, ��ư�� �����ִ� ��ũ��Ʈ �۵� -> selectBuilding ���� �ش� ������Ʈ ����.

        isBuildReady = true; // �Ǽ� �����·� ��ȯ
        //selectedBuilding = Instantiate(selectBuilding, this.transform.position, Quaternion.identity); //�ӽ� �ǹ�������Ʈ ���� (������ �ǹ��� �����ϰ�)
        
        selectedBuilding = Instantiate(selectBuilding, this.transform.position, Quaternion.identity, checkBuilding.transform); //�ӽ� �ǹ�������Ʈ ���� (������ �ǹ��� �����ϰ�)
        //selectedBuilding�� istrigger�� �ٲ�ߵ�.

        rigid = selectedBuilding.GetComponent<Rigidbody>(); // �ӽ� ������Ʈ�� rigidbody�� isKinematic���� ( �浹���� �Ұ��ϰ� )
        rigid.isKinematic = true;
        //selectedBuilding.transform.SetParent(checkBuilding.transform); // �ӽ� �ǹ� ������Ʈ�� �ݶ��̴� üũ�ϴ� ������Ʈ�� �ڽ����� �ִ´�.

        selectedBuilding.transform.position = checkBuilding.transform.position; // �ӽ� �ǹ� ������Ʈ�� ��ġ�� ���� (�浹�����ϴ� ������Ʈ�� ��ġ ���߱�)
        selectedBuilding.transform.rotation = checkBuilding.transform.rotation; // rotation���߱�
        selectedBuildingCollider = selectedBuilding.GetComponent<Collider>(); // �ӽ� �ǹ��� �ݶ��̴��� 
        selectedBuildingCollider.enabled = false; //����.(�ӽðǹ��� �浹�� �����Ǹ� �ȵ�)
        checkBuildingCollider = checkBuilding.GetComponent<BoxCollider>(); //�浹�����ϴ� ������Ʈ�� �ݶ��̴��� �����ͼ�

        Vector3 newSize = selectedBuilding.GetComponent<BoxCollider>().size; //�ӽ� ������Ʈ�� ũ�⸦ ���Ѵ���
        //checkBuilding.transform.localScale = new Vector3(1, 1, 1); // �˼����� �浹����������Ʈ�� Scale���� ������ �־ ������ scale�� �������״� (�ӽù���)
        checkBuildingCollider.size = newSize; //üũ �ݶ��̴� ������Ʈ�� ũ�⸦ �ӽ� ������Ʈ�� ũ��� �����ϰ� �����.    
        checkBuildingCollider.center = selectedBuilding.GetComponent<BoxCollider>().center;


        checkBuilding.SetActive(true); //�ǹ��� ���� ��ġ�� �ٸ� ��ü�� �ִ��� üũ�� collider Ȱ��ȭ
    }

}
