using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckCanBuild : MonoBehaviour
{

    // 기능 : 건축 가능한지 여부를 체크하는 컴포넌트. (layermask로 layer를 검사한다.)
    //건물들은 플레이어가 건설완성하기 전까진 incompletedBuilding 이다. 건물완성후에 해당 건물을 Building Layer로 변경하여야한다.
    public LayerMask checkLayer;
    public UnityEvent cantBuildState;
    public UnityEvent canBuildState;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
             
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other);

        if ((checkLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            cantBuildState.Invoke();
            Debug.Log("물체 겹침!");
        }

    }

    void OnTriggerStay(Collider other)
    {

        if ((checkLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            cantBuildState.Invoke();
            Debug.Log("물체 겹침! :" + other);
        }


    }
    void OnTriggerExit(Collider other)
    {
        canBuildState.Invoke(); 
        Debug.Log("물체 안겹침!");
    }
}
