using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateToMouse : MonoBehaviour
{

    [SerializeField] private float rotCamXAxisSpeed = 5.0f;
    [SerializeField] private float rotCamYAxisSpeed = 3.0f;

    public float limitMinX = -80.0f;
    public float limitMaxX = 50.0f;

    private float eulerAngleX;
    private float eulerAngleY;

    public void CalculateRotation(float mouseX, float mouseY)
    {
        eulerAngleY += mouseX * rotCamYAxisSpeed;
        eulerAngleX -= mouseY * rotCamYAxisSpeed;
        eulerAngleX = ClampAngle(eulerAngleX, limitMinX, limitMaxX);
        transform.rotation = Quaternion.Euler(eulerAngleX, eulerAngleY, 0);
    }

    private float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360) angle += 360;
        if (angle > 360) angle -= 360;
        
        return Mathf.Clamp(angle, min, max);
    }

}
