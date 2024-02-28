using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private RotateToMouse rotateToMouse; // 마우스 이동으로 카메라 회전

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
