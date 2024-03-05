using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wasdMoving : MonoBehaviour
{
    // Start is called before the first frame update
    public float moveSpeed = 5f;

    // Update is called once per frame
    void Update()
    {
        // 사용자 입력을 받아 이동 방향을 설정
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // 이동 방향에 따라 이동 벡터 계산
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * moveSpeed * Time.deltaTime;

        // 이동 벡터를 현재 위치에 더함
        transform.Translate(movement);
    }
}
