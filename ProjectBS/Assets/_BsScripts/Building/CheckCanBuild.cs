using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CheckCanBuild : MonoBehaviour
{

    // ��� : ���� �������� ���θ� üũ�ϴ� ������Ʈ. (layermask�� layer�� �˻��Ѵ�.)
    //�ǹ����� �÷��̾ �Ǽ��ϼ��ϱ� ������ incompletedBuilding �̴�. �ǹ��ϼ��Ŀ� �ش� �ǹ��� Building Layer�� �����Ͽ����Ѵ�.
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
            Debug.Log("��ü ��ħ!");
        }

    }

    void OnTriggerStay(Collider other)
    {

        if ((checkLayer.value & (1 << other.gameObject.layer)) != 0)
        {
            cantBuildState.Invoke();
            Debug.Log("��ü ��ħ!");
        }


    }
    void OnTriggerExit(Collider other)
    {
        canBuildState.Invoke(); 
        Debug.Log("��ü �Ȱ�ħ!");
    }
}
