using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private RotateToMouse rotateToMouse; // ���콺 �̵����� ī�޶� ȸ��

    void Awake()
    {
        rotateToMouse = GetComponent<RotateToMouse>();
    }

    void Update()
    {
        UpdateRotate();
    }

    void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        rotateToMouse.CalculateRotation(mouseX, mouseY);
    }
}
