using UnityEngine;
using UnityEngine.UI;

public class ConstructionKeyUI : FollowingTargetUI
{
    [SerializeField] private GameObject PressB;

    private void OnDisable()
    {
        transform.position = new Vector3(0, 100000, 0);
    }
}
