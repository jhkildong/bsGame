using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraAction : MonoBehaviour
{
    public Transform Target;

    private float offsetX = 0.0f;
    private float offsetY = 11.0f;
    private float offsetZ = -6.0f;

    public float cameraSpeed = 10.0f;
    Vector3 TargetPos;

    void Start()
    {
        transform.position = new Vector3(
            Target.position.x + offsetX,
            Target.position.y + offsetY,
            Target.position.z + offsetZ);
        transform.rotation = Quaternion.Euler(60, 0, 0);
    }

    void FixedUpdate()
    {
        if (Target == null)
            return;
        TargetPos = new Vector3(
            Target.position.x + offsetX,
            Target.position.y + offsetY,
            Target.position.z + offsetZ);
        transform.position = Vector3.Lerp(transform.position, TargetPos, 
            Time.deltaTime * cameraSpeed);
    }
}
