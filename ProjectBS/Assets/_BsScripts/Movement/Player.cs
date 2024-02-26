using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    private Rigidbody rigidBody;
    public float sp = 1.0f; //이동속도
    public float rotSpeed = 3.0f; //회전속도

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
        dir.x = Input.GetAxis("Horizontal"); //A, D키의 이동 방향
        dir.z = Input.GetAxis("Vertical"); //W, S키의 이동 방향
        dir.Normalize();
    }

    private void FixedUpdate()
    {
        if(dir != Vector3.zero)
        {
            // 회전 예외처리 - 정반대 방향으로 회전할때 어느쪽으로 회전해야 할지 몰라서 임의로 돌림
            if (Mathf.Sign(transform.forward.x) != Mathf.Sign(dir.x) || Mathf.Sign(transform.forward.z) != Mathf.Sign(dir.z))
            {
                transform.Rotate(0, 1, 0);
            }
            // 바라보는 방향으로 천천히 회전하는 코드
            transform.forward = Vector3.Lerp(transform.forward, dir, rotSpeed * Time.deltaTime);
        }
        // 이동하는 코드
        rigidBody.MovePosition(transform.position + dir * sp * Time.deltaTime);
    }

    
}
