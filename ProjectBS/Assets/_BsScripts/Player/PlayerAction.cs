using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Yeon;

public class PlayerAction : Player
{

    Movement myMovement;
    // Start is called before the first frame update
    private void Start()
    {
        Init();
        TryGetComponent(out myMovement);
    }
    Vector3 moveDir;
    Vector3 dir;
    Vector3 inputDir;
    public float angle;
    public LayerMask building;
    private void Update()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal"); //A, D키의 이동 방향
        moveDir.z = Input.GetAxisRaw("Vertical"); //W, S키의 이동 방향
        
        //현재 월드 기준 바라보는 방향의 각도
        angle = transform.rotation.eulerAngles.y;
        //바라보는 방향기준의 애니메이션 방향(입력받은 방향에서 바라보는 방향의 반대방향으로 회전)
        dir = Quaternion.AngleAxis(-angle , Vector3.up) * moveDir;


        inputDir = Vector3.Lerp(inputDir, dir, Time.deltaTime * 8.0f);
        inputDir.x = Mathf.Clamp(inputDir.x, -1.0f, 1.0f);
        inputDir.z = Mathf.Clamp(inputDir.z, -1.0f, 1.0f);

        if (inputDir.sqrMagnitude < 0.001f)
            inputDir = Vector3.zero;


        if(Input.GetMouseButton(0))
        {
            myAnim.SetTrigger(AnimParam.Attack);
        }
    }

    private void FixedUpdate()
    {
        myMovement.SetDirection(moveDir.normalized);
        myAnim.SetFloat("x", inputDir.x);
        myAnim.SetFloat("y", inputDir.z);
    }
}
