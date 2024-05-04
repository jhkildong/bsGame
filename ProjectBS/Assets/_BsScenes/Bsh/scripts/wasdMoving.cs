using System.Collections;
using UnityEngine;

public class wasdMoving : MonoBehaviour
{
    public Animator _myAnim;
    public float moveSpeed = 5f;
    void Update()
    {
        // ����� �Է��� �޾� �̵� ������ ����
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // �̵�
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * moveSpeed * Time.deltaTime;
        transform.position += movement;
    }
}
