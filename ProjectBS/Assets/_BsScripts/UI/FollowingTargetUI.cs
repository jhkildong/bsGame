using UnityEngine;

public abstract class FollowingTargetUI : UIComponent
{
    public Transform myTarget;
    public Vector3 currentPos;

    void Update()
    {
        if (myTarget != null)
            currentPos = myTarget.position;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(currentPos);
        if (screenPos.z > 0.0f)
        {
            transform.position = screenPos;
        }
        else
        {
            transform.position = new Vector3(0, 100000, 0);
        }
    }
}
