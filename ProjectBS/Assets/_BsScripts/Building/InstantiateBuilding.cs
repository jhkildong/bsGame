using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class InstantiateBuilding : MonoBehaviour
{
    //public BuildingScriptableObject setBuildingEvent;

    //기능 : UI버튼클릭시, SetBuilding.cs에서 ChangeStateToBuild를 호출하여 Build상태로 전환.
    //       마우스 위치에 클릭한 건물에 해당하는 가상의 건물을 생성하고, 마우스를 따라다니는 충돌감지 오브젝트를 SetActive한다.
    //       해당 건물은 collider가 있는 곳에 배치가 불가능하며(ground만 raycast로 검사)
    //       클릭시 해당 건물을 위치에 instantiate한다.
    //       건물은 IncompletedBuilding layer 상태로 생성되며, 해당 레이어는 충돌이 일어나지 않는다.
    public LayerMask mouseLayer;
    Rigidbody rigid;
    public GameObject selectBuilding; //선택한 건물 (UI 버튼을 누르면 해당 건물이 여기로 입력되어야함)
    public GameObject selectedBuilding;//건설위치를 정하기 위해 마우스를 따라다닐 표시를 위한 임시 건물
    Collider selectedBuildingCollider;//건설할 임시 건물의 콜라이더
    BoxCollider checkBuildingBoxCollider; // 건설가능 위치를 감지할 오브젝트의 BoxCollider
    SphereCollider checkBuildingSphereCollider; 
    CapsuleCollider checkBuildingCapsuleCollider;
    public GameObject checkBuilding; //건설 가능 위치를 감지할 오브젝트
    public Transform checkBuildingTransform; //건설 가능 위치를 감지할 오브젝트의 Transform 값. (scale 값을 사용하기 위해)
    public GameObject instBuilding; //실제로 생성될 건물
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
        checkBuildingBoxCollider = checkBuilding.GetComponent<BoxCollider>();
        checkBuildingSphereCollider = checkBuilding.GetComponent<SphereCollider>();
        checkBuildingCapsuleCollider = checkBuilding.GetComponent<CapsuleCollider>();
    }



    // Update is called once per frame
    void Update()
    {
        if (state == State.Normal) { 
           
        }
        if (state == State.Build) // UI클릭시 Build로 넘어온다. (Button Event에 있음)
        {
            if(!isBuildReady) 
            {
                GetReadyToBuild(); //건설 대기상태로.
                orgBuildingRenderer = selectBuilding.GetComponentsInChildren<Renderer>(); //원본 렌더러 색상,투명도 저장해두기.
            }
            mousePos = Input.mousePosition;
            Ray mouseRay = Camera.main.ScreenPointToRay(mousePos);
            if (Physics.Raycast(mouseRay, out RaycastHit hit, 1000f, mouseLayer)) // 마우스위치에 생성될 오브젝트를 보여준다. (임시 오브젝트가 따라다닌다)
            {
                //Debug.Log(hit.point);
                checkBuilding.transform.position = hit.point;
                //selectedBuilding.transform.position = selectedBuildingTransform.position;
                //selectedBuilding.transform.rotation = selectedBuildingTransform.rotation;
            }
            float wheel = -Input.GetAxis("Mouse ScrollWheel") * 300; //마우스 휠 돌린값 *300(회전속도)
            checkBuilding.transform.rotation *= Quaternion.Euler(0, wheel, 0);// 마우스 휠 돌린만큼 y축 기준으로 회전한다.

            //건설 가능,불가능 여부를 머터리얼 색깔로 나타내주는 코드
            GameObject _selectedBuilding = selectedBuilding;
            Renderer[] selectedBuildingRenderers = _selectedBuilding.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in selectedBuildingRenderers)
            {
                if (canBuild)
                {
                    renderer.material.color = new Color(0, 0.8f, 0, 0.5f); // 녹색, 투명도 0.5
                }
                else if (!canBuild)
                {
                    renderer.material.color = new Color(1f, 0, 0, 0.5f); //빨강, 투명도 0.5
                }
            }
            


            if (Input.GetMouseButtonDown(0)&&canBuild) // 건설가능한 위치에 클릭시.  //땅이 없는 구간에서(감지 못하는구간) ray는 Vector3.zero를 반환한다. (hit.point != Vector3.zero) 임시 방편
            {
                instBuilding = selectedBuilding; //생성될 건물은 선택한 건물이다.
                Renderer[] instBuildingRenderers = instBuilding.GetComponentsInChildren<Renderer>();
                for (int i = 0; i < orgBuildingRenderer.Length; i++)
                {
                    instBuildingRenderers[i].material.color = orgBuildingRenderer[i].sharedMaterial.color; // 원본 건물 색상으로 다시 변경
                }
                foreach (Renderer renderer in instBuildingRenderers)
                {
                    renderer.material.color = new Color(renderer.material.color.r, renderer.material.color.g, renderer.material.color.b, 0.3f); //투명도 0.5
                }
                selectedBuildingCollider.enabled = true; // 박스 콜라이더 다시 켜기.(콜라이더가 켜져도 Layer가 IncompletedBuilding 이기때문에 여전히 충돌 불가능)
                //rigid.isKinematic = false; // isKinematic도 다시 끈다.
                //건물이 설치되면서, instBuilding가 가진 컴포넌트 Building의 OnInstalled가 호출되어야한다. (건물이 설치된 상태로 변경) (커플링 발생 인터페이스로 수정할 예정)
                Building isInstalled  = instBuilding.GetComponent<Building>();
                isInstalled.OnInstalled();
                Instantiate(instBuilding, hit.point, checkBuilding.transform.rotation); //해당 위치에 회전한 그대로 생성
                EndBuild();
                
            }
            else if (Input.GetMouseButtonDown(0) && !canBuild)
            {
                Debug.Log("해당 위치에 건설할수 없습니다");
            }
            else if (Input.GetMouseButtonDown(1)) // 우클릭 입력시 건설 취소
            {

                EndBuild();
            }
        }       
    }

    /*
    public void IsInstalled() // 건물이 세팅될때(클릭으로 건설위치가 정해질때)
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
        isBuildReady = false; // 건설대기상태 끝.
        ChangeState(State.Normal);//일반상태로 돌아간다.
        Destroy(selectedBuilding);//임시 오브젝트 파괴
        checkBuildingBoxCollider.enabled = false;
        checkBuildingCapsuleCollider.enabled = false;
        checkBuildingSphereCollider.enabled = false;
        checkBuilding.SetActive(false);//콜라이더 체크 오브젝트 비활성화
    }

    public void GetReadyToBuild()
    {
        
        //건설 버튼이 눌리면, 버튼이 갖고있던 스크립트 작동 -> selectBuilding 으로 해당 오브젝트 전달.

        isBuildReady = true; // 건설 대기상태로 전환
        //selectedBuilding = Instantiate(selectBuilding, this.transform.position, Quaternion.identity); //임시 건물오브젝트 생성 (선택한 건물과 동일하게)


        selectedBuilding = Instantiate(selectBuilding, this.transform.position, Quaternion.identity, checkBuilding.transform); //임시 건물오브젝트 생성 (선택한 건물과 동일하게)
        //selectedBuilding을 istrigger로 바꿔야됨.

        rigid = selectedBuilding.GetComponent<Rigidbody>(); // 임시 오브젝트의 rigidbody를 isKinematic으로 ( 충돌감지 불가하게 )
        rigid.isKinematic = true;
        //selectedBuilding.transform.SetParent(checkBuilding.transform); // 임시 건물 오브젝트를 콜라이더 체크하는 오브젝트의 자식으로 넣는다.

        selectedBuilding.transform.position = checkBuilding.transform.position; // 임시 건물 오브젝트의 위치를 조정 (충돌감지하는 오브젝트와 위치 맞추기)
        selectedBuilding.transform.rotation = checkBuilding.transform.rotation; // rotation맞추기


        selectedBuildingCollider = selectedBuilding.GetComponent<Collider>(); // 임시 건물의 콜라이더를 
        selectedBuildingCollider.enabled = false; //끈다.(임시건물은 충돌이 감지되면 안됨)
        if(selectedBuildingCollider is BoxCollider)
        {
            //checkBuildingBoxCollider = checkBuilding.GetComponent<BoxCollider>(); //충돌감지하는 오브젝트의 콜라이더를 가져와서
            checkBuildingBoxCollider.enabled = true; // 콜라이더 체크 활성화. endbuild()에서 비활성화
            Vector3 newSize = selectedBuilding.GetComponent<BoxCollider>().size; //임시 오브젝트의 크기를 구한다음
                                                                                 //checkBuilding.transform.localScale = new Vector3(1, 1, 1); // 알수없는 충돌감지오브젝트의 Scale변동 문제가 있어서 강제로 scale을 고정시켰다 (임시방편)
            checkBuildingBoxCollider.size = newSize; //체크 콜라이더 오브젝트의 크기를 임시 오브젝트의 크기와 동일하게 맞춘다.    
            checkBuildingBoxCollider.center = selectedBuilding.GetComponent<BoxCollider>().center;
        }

        else if (selectedBuildingCollider is CapsuleCollider)
        {
            //checkBuildingCapsuleCollider = checkBuilding.GetComponent<CapsuleCollider>();
            checkBuildingCapsuleCollider.enabled=true;
            float radius = selectedBuilding.GetComponent<CapsuleCollider>().radius;
            float height = selectedBuilding.GetComponent<CapsuleCollider>().height;

            checkBuildingCapsuleCollider.radius = radius;
            checkBuildingCapsuleCollider.height = height;
            checkBuildingCapsuleCollider.center = selectedBuilding.GetComponent<CapsuleCollider>().center;
        }

        else if (selectedBuildingCollider is SphereCollider)
        {
            //checkBuildingSphereCollider = checkBuilding.GetComponent<SphereCollider>();
            checkBuildingSphereCollider.enabled = true;
            float radius = selectedBuilding.GetComponent<SphereCollider>().radius;

            checkBuildingSphereCollider.radius = radius;
            checkBuildingSphereCollider.center = selectedBuilding.GetComponent<SphereCollider>().center;
        }
        else
        {
            Debug.Log("건물의 콜라이더가 box,capsule,sphere가 아닙니다!");
        }
        checkBuilding.SetActive(true); //건물을 지을 위치에 다른 물체가 있는지 체크할 collider 활성화
    }

}
