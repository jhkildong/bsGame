using UnityEngine;

public abstract class FollowingTargetUI : UIComponent
{
    public Transform myTarget;

    void Update()
    {
        if (myTarget == null)
        {
            return;
        }
        Vector3 screenPos = Camera.main.WorldToScreenPoint(myTarget.position);
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
