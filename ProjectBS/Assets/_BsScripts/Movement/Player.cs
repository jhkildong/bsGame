using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody rigidBody;
    public float sp = 1.0f; //�̵��ӵ�
    public float rotSpeed = 3.0f; //ȸ���ӵ�

    private Vector3 dir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        if(!TryGetComponent(out rigidBody))
        {
            gameObject.AddComponent<Rigidbody>();
            rigidBody = GetComponent<Rigidbody>();
            rigidBody.freezeRotation = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        dir.x = Input.GetAxis("Horizontal"); //A, DŰ�� �̵� ����
        dir.z = Input.GetAxis("Vertical"); //W, SŰ�� �̵� ����
        dir.Normalize();
    }

    private void FixedUpdate()
    {
        if(dir != Vector3.zero)
        {
            // ȸ�� ����ó�� - ���ݴ� �������� ȸ���Ҷ� ��������� ȸ���ؾ� ���� ���� ���Ƿ� ����
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                transform.Rotate(0, 1, 0);
            }
            // �ٶ󺸴� �������� õõ�� ȸ���ϴ� �ڵ�
            transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
        }
        // �̵��ϴ� �ڵ�
        rigidBody.MovePosition(transform.position + dir * sp * Time.deltaTime);
    }

    
}
