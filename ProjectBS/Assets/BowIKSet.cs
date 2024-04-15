using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowIKSet : MonoBehaviour
{
    public Transform bowString;
    public Animator myAnim;
    public float ikWeight = 0.0f;
    [SerializeField] bool ikActive = false;

    private void OnAnimatorIK(int layerIndex)
    {
        myAnim.SetIKPosition(AvatarIKGoal.RightHand, bowString.position);
        if(ikActive)
        {
            myAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
        }
        else
        {
            myAnim.SetIKPositionWeight(AvatarIKGoal.RightHand, 0.0f);
        }
    }

    public void SetIK()
    {
        ikActive = true;
    }

    public void ClearIK()
    {
        ikActive = false;
    }
}
