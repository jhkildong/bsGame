using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public Camera cam;

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // 마우스 커서의 위치에서 Ray를 생성합니다.
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // Ray와 바닥의 교차점을 찾습니다.
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, (int)BSLayerMasks.Ground))
        {
            // 오브젝트를 교차점으로 이동시킵니다.
            transform.position = hit.point + Vector3.up * 0.1f;
        }
    }
}