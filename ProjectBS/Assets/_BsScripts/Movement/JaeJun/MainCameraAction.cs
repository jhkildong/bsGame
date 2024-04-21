using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraAction : MonoBehaviour
{
    public Transform Target;

    public float offsetX = 0.0f;
    public float offsetY = 10.0f;
    public float offsetZ = 10.0f;

    public float cameraSpeed = 10.0f;
    Vector3 TargetPos;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
