using System.Collections;
using UnityEngine;

public class wasdMoving : MonoBehaviour
{
    public Animator _myAnim;
    public float moveSpeed = 5f;
    void Update()
    {
        // 사용자 입력을 받아 이동 방향을 설정
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // 이동
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }
}
