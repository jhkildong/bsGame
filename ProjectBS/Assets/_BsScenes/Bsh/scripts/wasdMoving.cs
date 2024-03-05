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
        // ����� �Է��� �޾� �̵� ������ ����
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // �̵� ���⿡ ���� �̵� ���� ���
        Vector3 movement = new Vector3(moveHorizontal, 0f, moveVertical) * moveSpeed * Time.deltaTime;

        // �̵� ���͸� ���� ��ġ�� ����
        transform.Translate(movement);
    }
}
