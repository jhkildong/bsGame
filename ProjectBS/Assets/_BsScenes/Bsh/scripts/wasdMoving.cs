using System.Collections;
using UnityEngine;

public class wasdMoving : MonoBehaviour
{
    public float moveSpeed = 5f;
    void Update()
    {
        // ����� �Է��� �޾� �̵� ������ ����
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");

        // �̵�
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * moveSpeed * Time.deltaTime;
        transform.position += movement;

        // ȸ��
        if (movement != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, 360f * Time.deltaTime);
        }
    }
}
