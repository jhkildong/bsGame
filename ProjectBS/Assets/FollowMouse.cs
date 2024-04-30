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
        // ���콺 Ŀ���� ��ġ���� Ray�� �����մϴ�.
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);

        // Ray�� �ٴ��� �������� ã���ϴ�.
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, (int)BSLayerMasks.Ground))
        {
            // ������Ʈ�� ���������� �̵���ŵ�ϴ�.
            transform.position = hit.point + Vector3.up * 0.1f;
        }
    }
}