using UnityEngine;
using UnityEngine.UI;

public class ConstructionKeyUI : FollowingTargetUI
{
    public override int ID => _id;
    [SerializeField] private int _id;

    [SerializeField] private GameObject PressB;

    private void OnDisable()
    {
        transform.position = new Vector3(0, 100000, 0);
    }
}
